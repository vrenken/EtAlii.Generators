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

        public void WriteStateMachineInstantiation(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// Time to create a new state machine instance.");
            context.Writer.WriteLine($"_stateMachine = new {StatelessWriter.StateMachineType}(State.{StatelessWriter.BeginStateName});");
            context.Writer.WriteLine();

            WriteTriggerInstantiations(context);

            WriteStateInstantiations(context);
        }

        private void WriteTriggerInstantiations(WriteContext<StateMachine> context)
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

        private void WriteStateInstantiations(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// Then we need to configure the state machine.");

            var allStates = StateFragment.GetAllStates(context.Instance.StateFragments);
            foreach (var state in allStates)
            {
                WriteStateConstruction(context, state);
            }
        }
        private void WriteStateConstruction(WriteContext<StateMachine> context, string state)
        {
            context.Writer.WriteLine($"_stateMachine.Configure(State.{state})");

            var stateConfiguration = new List<string>();

            var isChoiceState = StateFragment.GetAllSuperStates(context.Instance.StateFragments)
                .Any(ss => ss.Name == state && ss.StereoType == StereoType.Choice);

            WriteEntryAndExitConfiguration(context, state, stateConfiguration, isChoiceState);
            WriteInboundTransitions(context, state, stateConfiguration, isChoiceState);
            WriteInternalTransitions(context, state, stateConfiguration);
            WriteOutboundTransitions(context, state, stateConfiguration);
            WriteSuperState(context, state, stateConfiguration);
            WriteSubstate(context, state, stateConfiguration);

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

        private void WriteSubstate(WriteContext<StateMachine> context, string state, List<string> stateConfiguration)
        {
            var superState = StateFragment.GetSuperState(context.Instance, state);
            if (superState != null)
            {
                stateConfiguration.Add($"\t.SubstateOf(State.{superState.Name})");
            }
        }

        private void WriteSuperState(WriteContext<StateMachine> context, string state, List<string> stateConfiguration)
        {
            var superState = StateFragment
                .GetAllSuperStates(context.Instance.StateFragments)
                .SingleOrDefault(s => s.Name == state);

            if (superState != null)
            {
                // Write initial transition (when needed)
                var unnamedInitialTransition = superState.StateFragments
                    .OfType<Transition>()
                    .SingleOrDefault(t => t.From == StatelessWriter.BeginStateName && !t.HasConcreteTriggerName);
                if (unnamedInitialTransition != null)
                {
                    stateConfiguration.Add($"\t.InitialTransition(State.{unnamedInitialTransition.To})");
                }

                var namedInitialTransitions = superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From == StatelessWriter.BeginStateName && t.HasConcreteTriggerName)
                    .ToArray();
                foreach(var namedInitialTransition in namedInitialTransitions)
                {
                    stateConfiguration.Add($"\t.Permit(Trigger.{namedInitialTransition.Trigger}, State.{namedInitialTransition.To})");
                }
            }
        }

        private void WriteEntryAndExitConfiguration(WriteContext<StateMachine> context, string state, List<string> stateConfiguration, bool isChoiceState)
        {
            var writeAsyncEntryConfiguration = StateFragment.HasOnlyAsyncInboundTransitions(context.Instance, state);
            if (writeAsyncEntryConfiguration)
            {
                var line = isChoiceState
                    ? $"\t.OnEntryAsync(() => On{state}Entered(new {state}EventArgs(_stateMachine)))"
                    : $"\t.OnEntryAsync(On{state}Entered)";
                stateConfiguration.Add(line);
            }
            else
            {
                var line = isChoiceState
                    ? $"\t.OnEntry(() => On{state}Entered(new {state}EventArgs(_stateMachine)))"
                    : $"\t.OnEntry(On{state}Entered)";
                stateConfiguration.Add(line);
            }

            var writeAsyncExitConfiguration = StateFragment.HasOnlyAsyncOutboundTransitions(context.Instance, state);
            if (writeAsyncExitConfiguration)
            {
                stateConfiguration.Add($"\t.OnExitAsync(On{state}Exited)");
            }
            else
            {
                stateConfiguration.Add($"\t.OnExit(On{state}Exited)");
            }
        }

        private void WriteInternalTransitions(WriteContext<StateMachine> context, string state, List<string> stateConfiguration)
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
        private void WriteOutboundTransitions(WriteContext<StateMachine> context, string state, List<string> stateConfiguration)
        {
            var lines = StateFragment
                .GetOutboundTransitions(context.Instance, state)
                .Select(transition => $"\t.Permit(Trigger.{transition.Trigger}, State.{transition.To})")
                .ToArray();
            stateConfiguration.AddRange(lines);
        }

        private void WriteInboundTransitions(WriteContext<StateMachine> context, string state, List<string> stateConfiguration, bool isChoiceState)
        {
            var lines = StateFragment
                .GetInboundTransitions(context.Instance.StateFragments, state)
                .Select(transition =>
                {
                    var triggerParameter = _transitionConverter.ToTriggerParameter(transition);
                    var genericParameters = _parameterConverter.ToGenericParameters(transition.Parameters);
                    var lambdaParameters = _parameterConverter.ToNamedVariables(transition.Parameters);
                    var parameters = transition.Parameters;
                    if (isChoiceState)
                    {
                        parameters = new [] { new Parameter($"", $"new {state}EventArgs(_stateMachine)", transition.Source) }
                            .Concat(parameters)
                            .ToArray();
                    }
                    var namedParameters = _parameterConverter.ToNamedVariables(parameters);
                    var transitionMethodName = _transitionConverter.ToTransitionMethodName(transition);

                    return isChoiceState
                        ? $"\t.OnEntryFrom{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, ({lambdaParameters}) => {transitionMethodName}({namedParameters}))"
                        : $"\t.OnEntryFrom{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, {transitionMethodName})";
                })
                .ToArray();
            stateConfiguration.AddRange(lines);
        }
    }
}
