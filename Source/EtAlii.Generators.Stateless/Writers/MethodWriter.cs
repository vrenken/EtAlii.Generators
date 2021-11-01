namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class MethodWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public MethodWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter, StateFragmentHelper stateFragmentHelper)
        {
            _transitionConverter = transitionConverter;
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
                var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters) => $"public void {triggerName}({typedParameters}) => _stateMachine.Fire{genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, syncTransitionSets, "sync", syncWrite);

                var asyncTransitions = _stateFragmentHelper.GetAsyncTransitions(context.Instance.StateFragments);
                var asyncTransitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(asyncTransitions, trigger);
                var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => _stateMachine.FireAsync{genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, asyncTransitionSets, "async", asyncWrite);
            }
        }

        public void WriteTriggerMethods(WriteContext<StateMachine> context, IEnumerable<Transition[]> transitionSets, string triggerType, Func<string, string, string, string, string, string> write)
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
            var allStates = _stateFragmentHelper.GetAllStates(context.Instance.StateFragments);
            foreach (var state in allStates)
            {
                WriteEntryAndExitMethods(context, state);

                var allTransitions = _stateFragmentHelper.GetAllTransitions(context.Instance.StateFragments);
                var uniqueTransitions = allTransitions
                    .Select(t => new { Transition = t, ParametersAsKey = $"{t.To}{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                    .GroupBy(item => item.ParametersAsKey)
                    .Select(g => g.First().Transition)
                    .ToArray();

                WriteInternalTransitionMethodsForState(context, uniqueTransitions, state);

                WriteInboundTransitionMethodsForState(context, uniqueTransitions, state);
            }
        }

        private void WriteEntryAndExitMethods(WriteContext<StateMachine> context, string state)
        {
            var writeAsyncEntryMethod = _stateFragmentHelper.HasOnlyAsyncInboundTransitions(context.Instance, state);

            var entryMethodName = $"On{state}Entered";
            context.Writer.WriteLine("/// <summary>");
            context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{state}' state.");
            if (writeAsyncEntryMethod)
            {
                context.Writer.WriteLine("/// <remark>");
                context.Writer.WriteLine("/// This method is configured to return a task because all transitions are marked to be called asynchronous.");
                context.Writer.WriteLine("/// </remark>");
            }
            context.Writer.WriteLine("/// </summary>");

            var parameters = Array.Empty<Parameter>();
            if (context.Instance.GenerateTriggerChoices)
            {
                parameters = new [] { new Parameter($"{state}EventArgs", "e", new SourcePosition(0, 0, "")) }
                    .Concat(parameters)
                    .ToArray();
            }

            var typedNamedParameters = _parameterConverter.ToTypedNamedVariables(parameters);

            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"partial {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({typedNamedParameters});");
            }
            else
            {
                context.Writer.WriteLine($"protected virtual {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({typedNamedParameters})");
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

            var writeAsyncExitMethod = _stateFragmentHelper.HasOnlyAsyncOutboundTransitions(context.Instance, state);
            var exitMethodName = $"On{state}Exited";
            context.Writer.WriteLine("/// <summary>");
            context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{state}' state.");
            if (writeAsyncEntryMethod)
            {
                context.Writer.WriteLine("/// <remark>");
                context.Writer.WriteLine("/// This method is configured to return a task because all transitions are marked to be called asynchronous.");
                context.Writer.WriteLine("/// </remark>");
            }
            context.Writer.WriteLine("/// </summary>");
            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"partial {(writeAsyncExitMethod ? "Task" : "void")} {exitMethodName}();");
            }
            else
            {
                context.Writer.WriteLine($"protected virtual {(writeAsyncExitMethod ? "Task" : "void")} {exitMethodName}()");
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

        private void WriteInboundTransitionMethodsForState(WriteContext<StateMachine> context, Transition[] uniqueTransitions, string state)
        {
            var inboundTransitions = uniqueTransitions
                .Where(t => t.From != t.To) // skip internal transitions.
                .Where(t => t.To == state)
                .ToArray();

            foreach (var transition in inboundTransitions)
            {
                var parameters = transition.Parameters;
                if (context.Instance.GenerateTriggerChoices)
                {
                    parameters = new[] { new Parameter($"{state}EventArgs", "e", new SourcePosition(0, 0, "")) }
                        .Concat(parameters)
                        .ToArray();
                }

                var typedNamedParameters = _parameterConverter.ToTypedNamedVariables(parameters);

                var transitionMethodName = _transitionConverter.ToTransitionMethodName(transition);
                WriteComment(context, new[] {transition}, "Implement this method to handle the transition below:");
                if (context.Instance.GeneratePartialClass)
                {
                    context.Writer.WriteLine($"partial {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters});");
                }
                else
                {
                    context.Writer.WriteLine($"protected virtual {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters})");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    if (transition.IsAsync)
                    {
                        context.Writer.WriteLine("return Task.CompletedTask;");
                    }

                    context.Writer.Indent -= 1;
                    context.Writer.WriteLine("}");
                }

                context.Writer.WriteLine();
            }
        }

        private void WriteInternalTransitionMethodsForState(WriteContext<StateMachine> context, Transition[] uniqueTransitions, string state)
        {
            var internalTransitions = uniqueTransitions
                .Where(t => t.From == t.To) // only internal transitions.
                .Where(t => t.To == state)
                .ToArray();

            foreach (var transition in internalTransitions)
            {
                var transitionMethodName = _transitionConverter.ToTransitionMethodName(transition);
                var typedNamedParameters1 = transition.Parameters.Any() ? $"{_parameterConverter.ToTypedNamedVariables(transition.Parameters)}, " : string.Empty;
                var typedNamedParameters2 = _parameterConverter.ToTypedNamedVariables(transition.Parameters);
                var namedParameters = _parameterConverter.ToNamedVariables(transition.Parameters);

                WriteComment(context, new[] {transition}, "Implement this method to handle the transition below:");
                if (context.Instance.GeneratePartialClass)
                {
                    context.Writer.WriteLine($"partial {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters2});");
                }
                else
                {
                    context.Writer.WriteLine($"protected virtual {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters2})");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    if (transition.IsAsync)
                    {
                        context.Writer.WriteLine("return Task.CompletedTask;");
                    }

                    context.Writer.Indent -= 1;
                    context.Writer.WriteLine("}");
                }
                context.Writer.WriteLine($"private {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters1}{StatelessWriter.StateMachineType}.Transition transition) => {transitionMethodName}({namedParameters});");
                context.Writer.WriteLine();
            }
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
