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

        public MethodWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter)
        {
            _transitionConverter = transitionConverter;
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

            var allTriggers = StateFragment.GetAllTriggers(context.Instance.StateFragments);
            foreach (var trigger in allTriggers)
            {
                WriteTransitionMethod(context, trigger);

                var syncTransitions = StateFragment.GetSyncTransitions(context.Instance.StateFragments);
                var syncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(syncTransitions, trigger);
                var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters)
                    => $"public void {triggerName}({typedParameters}) => RunOrQueueTransition(new SyncTransition({triggerName}Transition));// {genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, syncTransitionSets, "sync", syncWrite);

                var asyncTransitions = StateFragment.GetAsyncTransitions(context.Instance.StateFragments);
                var asyncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(asyncTransitions, trigger);
                var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters)
                    => $"public Task {triggerName}Async({typedParameters}) => RunOrQueueTransitionAsync(new AsyncTransition({triggerName}Transition));// {genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, asyncTransitionSets, "async", asyncWrite);
            }
        }

        private void WriteTransitionMethod(WriteContext<StateMachine> context, string trigger)
        {
            context.Writer.WriteLine($"private void {trigger}Transition()");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("switch (_state)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            var transitions = StateFragment.GetAllTransitions(context.Instance.StateFragments)
                .Where(t => t.Trigger == trigger)
                .ToArray();

            foreach (var transition in transitions)
            {
                WriteTransitionStateCase(context, transition);
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

        private void WriteTransitionStateCase(WriteContext<StateMachine> context, Transition transition)
        {
            var triggerVariableName = $"trigger{transition.From}";

            context.Writer.WriteLine($"case State.{transition.From}:");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"_state = State.{transition.To};");
            context.Writer.WriteLine($"Trigger {triggerVariableName} = new {transition.Trigger}Trigger();");

            context.Writer.WriteLine($"On{transition.From}Exited(({transition.Trigger}Trigger){triggerVariableName});");
            context.Writer.WriteLine($"On{transition.From}Exited({triggerVariableName});");
            var fromState = transition.From;
            while (StateFragment.GetSuperState(context.Instance, fromState) is var superState && superState != null)
            {
                context.Writer.WriteLine($"On{superState.Name}Exited({triggerVariableName});");
                fromState = superState.Name;
            }

            var enterLines = new List<string>();
            var toState = transition.To;
            while (StateFragment.GetSuperState(context.Instance, toState) is var superState && superState != null)
            {
                enterLines.Add($"On{superState.Name}Entered({triggerVariableName});");
                toState = superState.Name;
            }
            enterLines.Reverse();
            foreach (var enterLine in enterLines)
            {
                context.Writer.WriteLine(enterLine);
            }

            context.Writer.WriteLine($"On{transition.To}Entered({triggerVariableName});");
            context.Writer.WriteLine($"On{transition.To}Entered(({transition.Trigger}Trigger){triggerVariableName});");

            context.Writer.WriteLine("break;");
            context.Writer.Indent -= 1;
        }



        private void WriteTriggerMethods(WriteContext<StateMachine> context, Transition[][] transitionSets, string triggerType, Func<string, string, string, string, string, string> write)
        {
            foreach (var transitionSet in transitionSets)
            {
                var firstTransition = transitionSet.First();
                var parameters = firstTransition.Parameters;
                var typedParameters = _parameterConverter.ToTypedNamedVariables(parameters);
                var genericParameters = _parameterConverter.ToGenericParameters(parameters);
                var namedParameters = parameters.Any() ? $", {_parameterConverter.ToNamedVariables(parameters)}" : string.Empty;
                var triggerParameter = _transitionConverter.ToTriggerParameter(firstTransition);

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
            var allStates = StateFragment.GetAllStates(context.Instance.StateFragments);
            foreach (var state in allStates)
            {
                // var isChoiceState = StateFragment.GetAllSuperStates(context.Instance.StateFragments)
                //     .Any(ss => ss.Name == state && ss.StereoType == StereoType.Choice);

                var writeAsyncEntryMethod = StateFragment.HasOnlyAsyncInboundTransitions(context.Instance, state);

                WriteEntryMethod(context, state, null, writeAsyncEntryMethod);
                WriteExitMethod(context, state, null, writeAsyncEntryMethod);

                var inboundTransitions = StateFragment.GetInboundTransitions(context.Instance.StateFragments, state);
                foreach (var inboundTransition in inboundTransitions)
                {
                    WriteEntryMethod(context, state, inboundTransition.Trigger, writeAsyncEntryMethod);
                }
                var outboundTransitions = StateFragment.GetOutboundTransitions(context.Instance, state);
                foreach (var outboundTransition in outboundTransitions)
                {
                    WriteExitMethod(context, state, outboundTransition.Trigger, writeAsyncEntryMethod);
                }
                var internalTransitions = StateFragment.GetInternalTransitions(context.Instance.StateFragments, state);
                foreach (var internalTransition in internalTransitions)
                {
                    WriteEntryMethod(context, state, internalTransition.Trigger, writeAsyncEntryMethod);
                    WriteExitMethod(context, state, internalTransition.Trigger, writeAsyncEntryMethod);
                }
            }
        }

        private static void WriteExitMethod(WriteContext<StateMachine> context, string state, string trigger, bool writeAsyncEntryMethod)
        {
            var writeAsyncExitMethod = StateFragment.HasOnlyAsyncOutboundTransitions(context.Instance, state);
            var exitMethodName = $"On{state}Exited";
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

            var triggerName = trigger == null ? $"Trigger" : $"{trigger}Trigger";

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

        private void WriteEntryMethod(WriteContext<StateMachine> context, string state, string trigger, bool writeAsyncEntryMethod)
        {
            var entryMethodName = $"On{state}Entered";
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


            var triggerName = trigger == null ? "Trigger" : $"{trigger}Trigger";

            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"partial {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({triggerName} trigger);");
            }
            else
            {
                context.Writer.WriteLine($"protected virtual {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({triggerName} trigger)");
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
