namespace EtAlii.Generators.MicroMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class MethodWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public MethodWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter, IStateMachineLifetime lifetime, StateFragmentHelper stateFragmentHelper)
        {
            _transitionConverter = transitionConverter;
            _lifetime = lifetime;
            _stateFragmentHelper = stateFragmentHelper;
            _parameterConverter = parameterConverter;
        }

        /// <summary>
        /// Write the trigger methods through which the individual triggers can be fired.
        /// There is some magic involved in creating duplicates for cases where both async
        /// and sync methods are needed.
        /// Also the sequence of parameter types are important as C# won't allow methods with the
        /// same name, parameters with the same type but other names.
        /// </summary>
        public void WriteTriggerMethods(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The methods below can be each called to fire a specific trigger");
            context.Writer.WriteLine("// and cause the state machine to transition to another state.");
            context.Writer.WriteLine();

            var allTriggers = _stateFragmentHelper.GetAllTriggers(context.Instance.StateFragments);
            foreach (var trigger in allTriggers)
            {
                var syncTransitions = _stateFragmentHelper.GetSyncTransitions(context.Instance.StateFragments);
                var syncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(syncTransitions, trigger);
                var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters)
                    => $"public void {triggerName}({typedParameters}) => RunOrQueueTransition(new SyncTransition(() => {triggerName}Transition({namedParameters})));// {genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, syncTransitionSets, "sync", syncWrite);

                var asyncTransitions = _stateFragmentHelper.GetAsyncTransitions(context.Instance.StateFragments);
                var asyncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(asyncTransitions, trigger);
                var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters)
                    => $"public Task {triggerName}Async({typedParameters}) => RunOrQueueTransitionAsync(new AsyncTransition(() => {triggerName}Transition({namedParameters})));// {genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, asyncTransitionSets, "async", asyncWrite);
            }
        }

        private void WriteTransitionMethod(WriteContext<StateMachine> context, string trigger, string typedParameters, string namedParameters)
        {
            context.Writer.WriteLine($"private void {trigger}Transition({typedParameters})");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("switch (_state)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            var transitions = _stateFragmentHelper.GetAllTransitions(context.Instance.StateFragments)
                .Where(t => t.Trigger == trigger)
                .ToArray();

            foreach (var transition in transitions)
            {
                WriteTransitionStateCase(context, transition, namedParameters);
            }

            context.Writer.WriteLine("default:");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"throw new NotSupportedException($\"Trigger {trigger} is not supported in state {{_state}}\");");
            context.Writer.Indent -= 1;
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteTransitionStateCase(WriteContext<StateMachine> context, Transition transition, string namedParameters)
        {
            var parentSuperState = _stateFragmentHelper.GetSuperState(context.Instance, transition.To);
            var stateName = parentSuperState != null && transition.From == _lifetime.BeginStateName
                ? parentSuperState.Name
                : transition.From;

            var triggerVariableName = $"trigger{stateName}";
            var triggerTypeName = $"{transition.Trigger}Trigger";

            context.Writer.WriteLine($"case State.{stateName}:");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"_state = State.{transition.To};");
            context.Writer.WriteLine($"Trigger {triggerVariableName} = new {triggerTypeName}({namedParameters});");

            var chain = MethodChain.Create(context, _stateFragmentHelper, stateName, transition.Trigger, transition.To);

            foreach (var call in chain.ExitCalls)
            {
                if (!call.IsSuperState)
                {
                    context.Writer.WriteLine($"On{call.State}Exited(({triggerTypeName}){triggerVariableName});");
                }
                context.Writer.WriteLine($"On{call.State}Exited({triggerVariableName});");
            }
            foreach (var call in chain.EntryCalls)
            {
                var choice = context.Instance.GenerateTriggerChoices
                    ? $", _{_parameterConverter.ToCamelCase(call.State)}Choices"
                    : "";
                context.Writer.WriteLine($"On{call.State}Entered({triggerVariableName}{choice});");
                if (!call.IsSuperState)
                {
                    context.Writer.WriteLine($"On{call.State}Entered(({triggerTypeName}){triggerVariableName}{choice});");
                }
            }

            var toSuperState = _stateFragmentHelper
                .GetAllSuperStates(context.Instance.StateFragments)
                .SingleOrDefault(ss => ss.Name == transition.To);
            if (toSuperState != null)
            {
                var subTransitions = toSuperState.StateFragments
                    .OfType<Transition>()
                    .ToArray();

                var unnamedInboundTransition = subTransitions
                    .Where(t => !t.HasConcreteTriggerName)
                    .Where(t => t.Parameters.Length == 0)
                    .SingleOrDefault(t => t.From == _lifetime.BeginStateName);
                if (unnamedInboundTransition != null)
                {
                    var choice = context.Instance.GenerateTriggerChoices
                        ? $", _{_parameterConverter.ToCamelCase(unnamedInboundTransition.To)}Choices"
                        : "";

                    context.Writer.WriteLine("// We also need to activate the first substate.");
                    context.Writer.WriteLine($"_state = State.{unnamedInboundTransition.To};");
                    context.Writer.WriteLine($"On{unnamedInboundTransition.To}Entered({triggerVariableName}{choice});");
                }
            }

            context.Writer.WriteLine("break;");
            context.Writer.Indent -= 1;
        }

        public void WriteTriggerMethods(WriteContext<StateMachine> context, IEnumerable<Transition[]> transitionSets, string triggerType, Func<string, string, string, string, string, string> write, bool writeTransitionMethods = true)
        {
            foreach (var transitionSet in transitionSets)
            {
                var firstTransition = transitionSet.First();
                var parameters = firstTransition.Parameters;
                var typedParameters = _parameterConverter.ToTypedNamedVariables(parameters);
                var genericParameters = _parameterConverter.ToGenericParameters(parameters);
                var namedParameters = parameters.Any() ? _parameterConverter.ToNamedVariables(parameters) : string.Empty;
                var triggerParameter = _transitionConverter.ToTriggerParameter(firstTransition);

                if (writeTransitionMethods)
                {
                    WriteTransitionMethod(context, firstTransition.Trigger, typedParameters, namedParameters);
                }

                WriteComment(context, transitionSet, $"Depending on the current state, call this method to trigger one of the {triggerType} transitions below:");
                context.Writer.WriteLine(write(firstTransition.Trigger, typedParameters, genericParameters, triggerParameter, namedParameters));
                context.Writer.WriteLine();
            }
        }

        /// <summary>
        /// Write the transition methods through which behavior can be implemented for the state transitions.
        /// </summary>
        /// <param name="context"></param>
        public void WriteTransitionMethods(WriteContext<StateMachine> context)
        {
            var writtenMethods = new List<string>();

            var allStates = _stateFragmentHelper.GetAllStates(context.Instance.StateFragments);
            foreach (var state in allStates)
            {
                // var isChoiceState = StateFragment.GetAllSuperStates(context.Instance.StateFragments)
                //     .Any(ss egt ss.Name eq state and ss.StereoType eq StereoType.Choice)

                var writeAsyncEntryMethod = _stateFragmentHelper.HasOnlyAsyncInboundTransitions(context.Instance, state);

                WriteEntryMethod(context, state, null, writeAsyncEntryMethod, writtenMethods);
                WriteExitMethod(context, state, null, writeAsyncEntryMethod, writtenMethods);

                var inboundTransitions = _stateFragmentHelper.GetInboundTransitions(context.Instance.StateFragments, state);
                foreach (var inboundTransition in inboundTransitions)
                {
                    WriteEntryMethod(context, state, inboundTransition.Trigger, writeAsyncEntryMethod, writtenMethods);
                }
                var outboundTransitions = _stateFragmentHelper.GetOutboundTransitions(context.Instance, state);
                foreach (var outboundTransition in outboundTransitions)
                {
                    WriteExitMethod(context, state, outboundTransition.Trigger, writeAsyncEntryMethod, writtenMethods);
                }
                var internalTransitions = _stateFragmentHelper.GetInternalTransitions(context.Instance.StateFragments, state);
                foreach (var internalTransition in internalTransitions)
                {
                    WriteEntryMethod(context, state, internalTransition.Trigger, writeAsyncEntryMethod, writtenMethods);
                    WriteExitMethod(context, state, internalTransition.Trigger, writeAsyncEntryMethod, writtenMethods);
                }
            }
        }

        private void WriteExitMethod(WriteContext<StateMachine> context, string state, string trigger, bool writeAsyncEntryMethod, List<string> writtenMethods)
        {
            var writeAsyncExitMethod = _stateFragmentHelper.HasOnlyAsyncOutboundTransitions(context.Instance, state);
            var exitMethodName = $"On{state}Exited";
            var triggerName = trigger == null ? $"Trigger" : $"{trigger}Trigger";

            var key = $"{exitMethodName}({triggerName} trigger)";
            if (writtenMethods.Contains(key))
            {
                return;
            }
            writtenMethods.Add(key);

            context.Writer.WriteLine("/// <summary>");
            if (trigger == null)
            {
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{state}' state.");
            }
            else
            {
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{state}' state by the '{trigger}' trigger.");
            }
            if (writeAsyncEntryMethod)
            {
                context.Writer.WriteLine("/// <remark>");
                context.Writer.WriteLine("/// This method is configured to return a task because all transitions are marked to be called asynchronous.");
                context.Writer.WriteLine("/// </remark>");
            }
            context.Writer.WriteLine("/// </summary>");

            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"partial {(writeAsyncExitMethod ? "Task" : "void")} {exitMethodName}({triggerName} trigger);");
            }
            else
            {
                context.Writer.WriteLine($"protected virtual {(writeAsyncExitMethod ? "Task" : "void")} {exitMethodName}({triggerName} trigger)");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                if (writeAsyncExitMethod)
                {
                    context.Writer.WriteLine("return Task.CompletedTask;");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
            }

            context.Writer.WriteLine();
        }

        private void WriteEntryMethod(WriteContext<StateMachine> context, string state, string trigger, bool writeAsyncEntryMethod, List<string> writtenMethods)
        {
            var entryMethodName = $"On{state}Entered";
            var triggerName = trigger == null ? "Trigger" : $"{trigger}Trigger";

            var key = $"{entryMethodName}({triggerName} trigger)";
            if (writtenMethods.Contains(key))
            {
                return;
            }
            writtenMethods.Add(key);

            context.Writer.WriteLine("/// <summary>");
            if (trigger == null)
            {
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{state}' state.");
            }
            else
            {
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{state}' state by the '{trigger}' trigger.");
            }
            if (writeAsyncEntryMethod)
            {
                context.Writer.WriteLine("/// <remark>");
                context.Writer.WriteLine("/// This method is configured to return a task because all transitions are marked to be called asynchronous.");
                context.Writer.WriteLine("/// </remark>");
            }

            context.Writer.WriteLine("/// </summary>");


            var choices = context.Instance.GenerateTriggerChoices
                ? $", {state}Choices choices"
                : "";

            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"partial {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({triggerName} trigger{choices});");
            }
            else
            {
                context.Writer.WriteLine($"protected virtual {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({triggerName} trigger{choices})");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                if (writeAsyncEntryMethod)
                {
                    context.Writer.WriteLine("return Task.CompletedTask;");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
            }

            context.Writer.WriteLine();
        }

        private void WriteComment(WriteContext<StateMachine> context, Transition[] transitionSet, string message)
        {
            context.Writer.WriteLine($"/// <summary>");
            context.Writer.WriteLine($"/// {message}<br/>");
            foreach (var transition in transitionSet)
            {
                context.Writer.WriteLine($"/// {transition.From} --&gt; {transition.To} : {transition.Trigger}<br/>");
            }
            context.Writer.WriteLine($"/// </summary>");
        }
    }
}
