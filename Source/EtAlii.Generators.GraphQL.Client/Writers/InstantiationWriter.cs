namespace EtAlii.Generators.GraphQL.Client
{
    using System.Collections.Generic;
    using System.Linq;

    public class InstantiationWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;

        public InstantiationWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter)
        {
            _parameterConverter = parameterConverter;
            _transitionConverter = transitionConverter;
        }

        public void WriteStateMachineInstantiation(WriteContext context)
        {
            context.Writer.WriteLine("// Time to create a new state machine instance.");
            context.Writer.WriteLine($"_stateMachine = new {SourceGenerator.StateMachineType}(State.{SourceGenerator.BeginStateName});");
            context.Writer.WriteLine();

            WriteTriggerInstantiations(context);

            WriteStateInstantiations(context);
        }

        private void WriteTriggerInstantiations(WriteContext context)
        {

            // We only need to write a trigger construction calls for all relations that have parameters.
            var uniqueTransitionsWithParameters = StateFragment.GetUniqueParameterTransitions(context.StateMachine.StateFragments)
                .Where(t => t.Parameters.Any())
                .ToArray();

            if (uniqueTransitionsWithParameters.Any())
            {
                context.Writer.WriteLine("// And for all triggers that are configured with parameters we need to instantiate the corresponding backing fields.");

                foreach (var uniqueTransition in uniqueTransitionsWithParameters)
                {
                    var genericParameters = _parameterConverter.ToGenericParameters(uniqueTransition.Parameters);
                    var triggerMemberName = _transitionConverter.ToTriggerMemberName(uniqueTransition);

                    context.Writer.WriteLine($"{triggerMemberName} = _stateMachine.SetTriggerParameters{genericParameters}(Trigger.{uniqueTransition.Trigger});");
                }

                context.Writer.WriteLine();
            }
        }

        private void WriteStateInstantiations(WriteContext context)
        {
            context.Writer.WriteLine("// Then we need to configure the state machine.");

            var allStates = StateFragment.GetAllStates(context.StateMachine.StateFragments);
            foreach (var state in allStates)
            {
                WriteStateConstruction(context, state);
            }
        }
        private void WriteStateConstruction(WriteContext context, string state)
        {
            context.Writer.WriteLine($"_stateMachine.Configure(State.{state})");

            var stateConfiguration = new List<string>();

            var superState = context.StateMachine.StateFragments
                .OfType<SuperState>()
                .SingleOrDefault(s => s.StateFragments.OfType<Transition>().Any(t => t.To == state));
            if (superState != null)
            {
                context.Writer.WriteLine($"\t.SubstateOf(State.{superState.Name})");
            }

            WriteEntryAndExitConfiguration(context, state, stateConfiguration);
            WriteInboundTransitions(context, state, stateConfiguration);
            WriteInternalTransitions(context, state, stateConfiguration);
            WriteOutboundTransitions(context, state, stateConfiguration);

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

        private void WriteEntryAndExitConfiguration(WriteContext context, string state, List<string> stateConfiguration)
        {
            var inboundTransitions = StateFragment.GetInboundTransitions(context.StateMachine.StateFragments, state);
            if (inboundTransitions.All(t => t.IsAsync) && state != SourceGenerator.BeginStateName && state != SourceGenerator.EndStateName)
            {
                stateConfiguration.Add($"\t.OnEntryAsync(On{state}EnteredAsync)");
            }
            else
            {
                stateConfiguration.Add($"\t.OnEntry(On{state}Entered)");
            }

            var outboundTransitions = StateFragment.GetOutboundTransitions(context.StateMachine.StateFragments, state);
            if (outboundTransitions.All(t => t.IsAsync) && state != SourceGenerator.BeginStateName && state != SourceGenerator.EndStateName)
            {
                stateConfiguration.Add($"\t.OnExitAsync(On{state}ExitedAsync)");
            }
            else
            {
                stateConfiguration.Add($"\t.OnExit(On{state}Exited)");
            }
        }

        private void WriteInternalTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var internalTransitions = StateFragment.GetInternalTransitions(context.StateMachine.StateFragments, state);

            var lines = internalTransitions
                .GroupBy(t => t.Trigger)
                .Select(g => g.First())
                .Select(transition =>
                {
                    var triggerParameter = _transitionConverter.ToTriggerParameter(transition);
                    var genericParameters = _parameterConverter.ToGenericParameters(transition.Parameters);
                    var transitionMethodName = _transitionConverter.ToTransitionMethodName(transition);
                    var triggerParameterTypes = string.Join(", ", transition.Parameters.Select(p => p.Type));
                    triggerParameterTypes = transition.Parameters.Any() ? $"{triggerParameterTypes}, " : "";
                    var methodCast = transition.IsAsync
                        ? $"(Func<{triggerParameterTypes}{SourceGenerator.StateMachineType}.Transition, Task>)"
                        : $"(Action<{triggerParameterTypes}{SourceGenerator.StateMachineType}.Transition>)";
                    return $"\t.InternalTransition{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, {methodCast}{transitionMethodName})";
                })
                .ToArray();

            stateConfiguration.AddRange(lines);
        }
        private void WriteOutboundTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var outboundTransitions = StateFragment.GetOutboundTransitions(context.StateMachine.StateFragments, state);
            var lines = outboundTransitions
                .GroupBy(t => t.Trigger)
                .Select(g => g.First())
                .Select(transition => $"\t.Permit(Trigger.{transition.Trigger}, State.{transition.To})")
                .ToArray();

            stateConfiguration.AddRange(lines);
        }

        private void WriteInboundTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var inboundTransitions = StateFragment.GetInboundTransitions(context.StateMachine.StateFragments, state);
            var lines = inboundTransitions
                .GroupBy(t => t.Trigger)
                .Select(g => g.First())
                .Select(transition =>
                {
                    var triggerParameter = _transitionConverter.ToTriggerParameter(transition);
                    var genericParameters = _parameterConverter.ToGenericParameters(transition.Parameters);
                    var transitionMethodName = _transitionConverter.ToTransitionMethodName(transition);
                    return $"\t.OnEntryFrom{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, {transitionMethodName})";
                })
                .ToArray();

            stateConfiguration.AddRange(lines);
        }
    }
}
