namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class InstantiationWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public InstantiationWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter, IStateMachineLifetime lifetime, StateFragmentHelper stateFragmentHelper)
        {
            _parameterConverter = parameterConverter;
            _transitionConverter = transitionConverter;
            _lifetime = lifetime;
            _stateFragmentHelper = stateFragmentHelper;
        }

        public void WriteStateMachineInstantiation(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// Time to create a new state machine instance.");
            context.Writer.WriteLine($"_stateMachine = new {StatelessWriter.StateMachineType}(State.{_lifetime.BeginStateName});");
            context.Writer.WriteLine();

            WriteTriggerInstantiations(context);

            WriteStateInstantiations(context);
        }

        private void WriteTriggerInstantiations(WriteContext<StateMachine> context)
        {
            // We only need to write a trigger construction calls for all relations that have parameters.
            var uniqueTransitionsWithParameters = _stateFragmentHelper.GetUniqueParameterTransitions(context.Instance.StateFragments)
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

            foreach (var state in context.Instance.SequentialStates)
            {
                WriteStateConstruction(context, state);
            }
        }
        private void WriteStateConstruction(WriteContext<StateMachine> context, State state)
        {
            context.Writer.WriteLine($"_stateMachine.Configure(State.{state.Name})");

            var stateConfiguration = new List<string>();

            WriteEntryAndExitConfiguration(context, state, stateConfiguration);
            WriteInboundTransitions(context, state, stateConfiguration);
            WriteInternalTransitions(state, stateConfiguration);
            WriteOutboundTransitions(state, stateConfiguration);
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

        private void WriteSubstate(WriteContext<StateMachine> context, State state, List<string> stateConfiguration)
        {
            var superState = _stateFragmentHelper.GetSuperState(context.Instance, state.Name);
            if (superState != null)
            {
                stateConfiguration.Add($"\t.SubstateOf(State.{superState.Name})");
            }
        }

        private void WriteSuperState(WriteContext<StateMachine> context, State state, List<string> stateConfiguration)
        {
            var superState = context.Instance.AllSuperStates
                .SingleOrDefault(s => s.Name == state.Name);

            if (superState != null)
            {
                // Write initial transition (when needed)
                var unnamedInitialTransition = superState.StateFragments
                    .OfType<Transition>()
                    .SingleOrDefault(t => t.From == _lifetime.BeginStateName && !t.HasConcreteTriggerName);
                if (unnamedInitialTransition != null)
                {
                    stateConfiguration.Add($"\t.InitialTransition(State.{unnamedInitialTransition.To})");
                }

                var namedInitialTransitions = superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From == _lifetime.BeginStateName && t.HasConcreteTriggerName)
                    .ToArray();
                foreach(var namedInitialTransition in namedInitialTransitions)
                {
                    stateConfiguration.Add($"\t.Permit(Trigger.{namedInitialTransition.Trigger}, State.{namedInitialTransition.To})");
                }
            }
        }

        private void WriteEntryAndExitConfiguration(WriteContext<StateMachine> context, State state, List<string> stateConfiguration)
        {
            var writeAsyncEntryConfiguration = state.HasOnlyAsyncInboundTransitions;
            if (writeAsyncEntryConfiguration)
            {
                var line = context.Instance.GenerateTriggerChoices
                    ? $"\t.OnEntryAsync(() => On{state.Name}Entered(new {state.Name}EventArgs(this)))"
                    : $"\t.OnEntryAsync(On{state.Name}Entered)";
                stateConfiguration.Add(line);
            }
            else
            {
                var line = context.Instance.GenerateTriggerChoices
                    ? $"\t.OnEntry(() => On{state.Name}Entered(new {state.Name}EventArgs(this)))"
                    : $"\t.OnEntry(On{state.Name}Entered)";
                stateConfiguration.Add(line);
            }

            var writeAsyncExitConfiguration = state.HasOnlyAsyncOutboundTransitions;
            if (writeAsyncExitConfiguration)
            {
                stateConfiguration.Add($"\t.OnExitAsync(On{state.Name}Exited)");
            }
            else
            {
                stateConfiguration.Add($"\t.OnExit(On{state.Name}Exited)");
            }
        }

        private void WriteInternalTransitions(State state, List<string> stateConfiguration)
        {
            var lines = state.InternalTransitions
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
        private void WriteOutboundTransitions(State state, List<string> stateConfiguration)
        {
            var lines = state.OutboundTransitions
                .Select(transition => $"\t.Permit(Trigger.{transition.Trigger}, State.{transition.To})")
                .ToArray();
            stateConfiguration.AddRange(lines);
        }

        private void WriteInboundTransitions(WriteContext<StateMachine> context, State state, List<string> stateConfiguration)
        {
            var lines = state.InboundTransitions
                .Select(transition =>
                {
                    var triggerParameter = _transitionConverter.ToTriggerParameter(transition);
                    var genericParameters = _parameterConverter.ToGenericParameters(transition.Parameters);
                    var lambdaParameters = _parameterConverter.ToNamedVariables(transition.Parameters);
                    var parameters = transition.Parameters;
                    if (context.Instance.GenerateTriggerChoices)
                    {
                        parameters = new [] { new Parameter($"", $"new {state.Name}EventArgs(this)", transition.Source) }
                            .Concat(parameters)
                            .ToArray();
                    }
                    var namedParameters = _parameterConverter.ToNamedVariables(parameters, context.Instance.GenerateTriggerChoices ? 1 : 0);
                    var transitionMethodName = _transitionConverter.ToTransitionMethodName(transition);

                    return context.Instance.GenerateTriggerChoices
                        ? $"\t.OnEntryFrom{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, ({lambdaParameters}) => {transitionMethodName}({namedParameters}))"
                        : $"\t.OnEntryFrom{(transition.IsAsync ? "Async" : "")}{genericParameters}({triggerParameter}, {transitionMethodName})";
                })
                .ToArray();
            stateConfiguration.AddRange(lines);
        }
    }
}
