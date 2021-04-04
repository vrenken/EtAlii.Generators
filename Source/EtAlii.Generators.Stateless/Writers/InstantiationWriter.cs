namespace EtAlii.Generators.Stateless
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
            context.Writer.WriteLine($"_stateMachine = new {StatelessWriter.StateMachineType}(State.{StatelessWriter.BeginStateName});");
            context.Writer.WriteLine();

            WriteTriggerInstantiations(context);

            WriteStateInstantiations(context);
        }

        private void WriteTriggerInstantiations(WriteContext context)
        {
            // We only need to write a trigger construction calls for all relations that have parameters.
            var uniqueTransitionsWithParameters = StateFragment.GetUniqueParameterTransitions(context.Instance.StateFragments)
                .Where(t => t.Parameters.Any())
                .ToArray();

            if (uniqueTransitionsWithParameters.Any())
            {
                context.Writer.WriteLine("// And for all triggers that are configured with parameters we need to instantiate the corresponding backing fields.");

                foreach (var transition in uniqueTransitionsWithParameters)
                {
                    var genericParameters = _parameterConverter.ToGenericParameters(transition.Parameters);
                    var triggerMemberName = _transitionConverter.ToTriggerMemberName(transition);

                    context.Writer.WriteLine($"{triggerMemberName} = _stateMachine.SetTriggerParameters{genericParameters}(Trigger.{transition.Trigger});");
                }

                context.Writer.WriteLine();
            }
        }

        private void WriteStateInstantiations(WriteContext context)
        {
            context.Writer.WriteLine("// Then we need to configure the state machine.");

            var allStates = StateFragment.GetAllStates(context.Instance.StateFragments);
            foreach (var state in allStates)
            {
                WriteStateConstruction(context, state);
            }
        }
        private void WriteStateConstruction(WriteContext context, string state)
        {
            context.Writer.WriteLine($"_stateMachine.Configure(State.{state})");

            var stateConfiguration = new List<string>();

            var superState = StateFragment.GetSuperState(context.Instance.StateFragments, state);
            if (superState != null)
            {
                context.Writer.WriteLine($"\t.SubstateOf(State.{superState.Name})");
            }

            WriteEntryAndExitConfiguration(context, state, stateConfiguration);
            WriteInboundTransitions(context, state, stateConfiguration);
            WriteInternalTransitions(context, state, stateConfiguration);
            WriteOutboundTransitions(context, state, stateConfiguration);

            stateConfiguration = stateConfiguration.OrderBy(l => l).ToList();
            // ReSharper disable UseIndexFromEndExpression - Roslyn generators need an old version of C# that does not support index from end.
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
            var inboundTransitions = StateFragment.GetInboundTransitions(context.Instance.StateFragments, state);
            if (inboundTransitions.Any() && inboundTransitions.All(t => t.IsAsync) && state != StatelessWriter.BeginStateName && state != StatelessWriter.EndStateName)
            {
                stateConfiguration.Add($"\t.OnEntryAsync(On{state}EnteredAsync)");
            }
            else
            {
                stateConfiguration.Add($"\t.OnEntry(On{state}Entered)");
            }

            var outboundTransitions = StateFragment.GetOutboundTransitions(context.Instance, state);
            if (outboundTransitions.Any() && outboundTransitions.All(t => t.IsAsync) && state != StatelessWriter.BeginStateName && state != StatelessWriter.EndStateName)
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
            var lines = StateFragment
                .GetInternalTransitions(context.Instance.StateFragments, state)
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
                        ? $"(Func<{triggerParameterTypes}{StatelessWriter.StateMachineType}.Transition, Task>)"
                        : $"(Action<{triggerParameterTypes}{StatelessWriter.StateMachineType}.Transition>)";
                    return $"\t.InternalTransition{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, {methodCast}{transitionMethodName})";
                })
                .ToArray();
            stateConfiguration.AddRange(lines);
        }
        private void WriteOutboundTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var lines = StateFragment
                .GetOutboundTransitions(context.Instance, state)
                .Select(transition => $"\t.Permit(Trigger.{transition.Trigger}, State.{transition.To})")
                .ToArray();
            stateConfiguration.AddRange(lines);
        }

        private void WriteInboundTransitions(WriteContext context, string state, List<string> stateConfiguration)
        {
            var lines = StateFragment
                .GetInboundTransitions(context.Instance.StateFragments, state)
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
