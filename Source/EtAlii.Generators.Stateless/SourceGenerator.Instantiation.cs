namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class SourceGenerator
    {
        private const string StateMachineType = "global::Stateless.StateMachine<State, Trigger>";

        private void WriteTriggerConstructions(WriteContext context)
        {
            // We only need to write a trigger construction calls for all relations that have parameters.
            var uniqueTransitions = context.UniqueParameterTransitions
                .Where(t => t.Parameters.Any())
                .ToArray();

            if (uniqueTransitions.Any())
            {
                context.Writer.WriteLine("// And for all triggers that are configured with parameters we need to instantiate the corresponding backing fields.");
            }

            foreach (var uniqueTransition in uniqueTransitions)
            {
                var genericParameters = ToGenericParameters(uniqueTransition.Parameters);
                var triggerMemberName = ToTriggerMemberName(uniqueTransition);

                context.Writer.WriteLine($"{triggerMemberName} = _stateMachine.SetTriggerParameters{genericParameters}(Trigger.{uniqueTransition.Trigger});");
            }

            if (uniqueTransitions.Any())
            {
                context.Writer.WriteLine();
            }
        }

        private void WriteStateConstructions(WriteContext context)
        {
            context.Writer.WriteLine("// Then we need to configure the state machine.");

            foreach (var state in context.AllStates)
            {
                WriteStateConstruction(context, state);
            }
        }
        private void WriteStateConstruction(WriteContext context, string state)
        {
            context.Writer.WriteLine($"_stateMachine.Configure(State.{state})");

            var stateConfiguration = new List<string>();

            WriteOutboundTransitions(context, state, stateConfiguration);

            stateConfiguration.Add($"\t.OnEntry(On{state}Entered)");
            stateConfiguration.Add($"\t.OnExit(On{state}Exited)");

            WriteInboundTransitions(context, state, stateConfiguration);

            stateConfiguration = stateConfiguration.OrderBy(l => l).ToList();
            // ReSharper disable UseIndexFromEndExpression
            stateConfiguration[stateConfiguration.Count - 1] = stateConfiguration[stateConfiguration.Count - 1] + ";";
            // ReSharper restore UseIndexFromEndExpression

            foreach (var line in stateConfiguration)
            {
                context.Writer.WriteLine(line);
            }

            context.Writer.WriteLine();
        }

        private void WriteOutboundTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var outboundTransitions = context.AllTransitions
                .Where(t => t.From == state)
                .ToArray();

            var lines = outboundTransitions
                .GroupBy(t => t.Trigger)
                .Select(g => g.First())
                .Select(transition =>
                {
                    if (transition.From == transition.To)
                    {
                        var triggerParameter = ToTriggerParameter(transition);
                        var genericParameters = ToGenericParameters(transition.Parameters);
                        var transitionMethodName = ToTransitionMethodName(transition);
                        var triggerParameterTypes = string.Join(", ", transition.Parameters.Select(p => p.Type));
                        triggerParameterTypes = transition.Parameters.Any() ? $"{triggerParameterTypes}, " : "";
                        return $"\t.InternalTransition{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, (Action<{triggerParameterTypes}{StateMachineType}.Transition>){transitionMethodName})";
                    }
                    else
                    {
                        return $"\t.Permit(Trigger.{transition.Trigger}, State.{transition.To})";
                    }
                })
                .ToArray();

            stateConfiguration.AddRange(lines);
        }

        private void WriteInboundTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var inboundTransitions = context.AllTransitions
                .Where(t => t.To == state)
                .ToArray();

            var lines = inboundTransitions
                .GroupBy(t => t.Trigger)
                .Select(g => g.First())
                .Where(t => t.From != t.To) // No need to write Entries for the internal transitions again.
                .Select(transition =>
                {
                    var triggerParameter = ToTriggerParameter(transition);
                    var genericParameters = ToGenericParameters(transition.Parameters);
                    var transitionMethodName = ToTransitionMethodName(transition);
                    return $"\t.OnEntryFrom{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, {transitionMethodName})";
                })
                .ToArray();

            stateConfiguration.AddRange(lines);
        }
    }
}
