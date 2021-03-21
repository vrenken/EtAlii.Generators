namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Linq;

    public partial class SourceGenerator
    {
        /// <summary>
        /// Write the trigger methods through which the individual triggers can be fired.
        /// There is some magic involved in creating duplicates for cases where both async
        /// and sync methods are needed.
        /// Also the sequence of parameter types are important as C# won't allow methods with the
        /// same name, parameters with the same type but other names.
        /// </summary>
        private void WriteTriggerMethods(WriteContext context)
        {
            context.Writer.WriteLine("// The methods below can be each called to fire a specific trigger");
            context.Writer.WriteLine("// and cause the state machine to transition to another state.");
            context.Writer.WriteLine();

            foreach (var trigger in context.AllTriggers)
            {
                var syncTransitions = context.AllTransitions
                    .Where(t => !t.IsAsync)
                    .ToArray();

                var syncTransitionSets = ToTransitionsSetsPerTriggerAndUniqueParameters(syncTransitions, trigger);

                var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters) => $"public void {triggerName}({typedParameters}) => _stateMachine.Fire{genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, syncTransitionSets, "sync", syncWrite);

                var asyncTransitions = context.AllTransitions
                    .Where(t => t.IsAsync)
                    .ToArray();

                var asyncTransitionSets = ToTransitionsSetsPerTriggerAndUniqueParameters(asyncTransitions, trigger);
                var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, genericParameters, triggerParameter, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => _stateMachine.FireAsync{genericParameters}({triggerParameter}{namedParameters});");
                WriteTriggerMethods(context, asyncTransitionSets, "async", asyncWrite);
            }
        }

        private void WriteTriggerMethods(WriteContext context, StateTransition[][] transitionSets, string triggerType, Func<string, string, string, string, string, string> write)
        {
            foreach (var transitionSet in transitionSets)
            {
                var firstTransition = transitionSet.First();
                var parameters = firstTransition.Parameters;
                var typedParameters = ToTypedNamedVariables(parameters);
                var genericParameters = ToGenericParameters(parameters);
                ;
                var namedParameters = parameters.Any() ? $", {ToNamedVariables(parameters)}" : string.Empty;
                var triggerParameter = ToTriggerParameter(firstTransition);

                WriteComment(context, transitionSet, $"Depending on the current state, call this method to trigger one of the {triggerType} transitions below:");
                context.Writer.WriteLine(write(firstTransition.Trigger, typedParameters, genericParameters, triggerParameter, namedParameters));
                context.Writer.WriteLine();
            }
        }

        /// <summary>
        /// Write the transition methods through which behavior can be implemented for the state transitions.
        /// </summary>
        /// <param name="context"></param>
        private void WriteTransitionMethods(WriteContext context)
        {
            foreach (var state in context.AllStates)
            {
                context.Writer.WriteLine("/// <summary>");
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{state}' state.");
                context.Writer.WriteLine("/// </summary>");
                context.Writer.WriteLine($"protected virtual void On{state}Entered()");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();

                context.Writer.WriteLine("/// <summary>");
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{state}' state.");
                context.Writer.WriteLine("/// </summary>");
                context.Writer.WriteLine($"protected virtual void On{state}Exited()");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();

                var uniqueTransitions = context.AllTransitions
                    .Select(t => new { Transition = t, ParametersAsKey = $"{t.To}{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                    .GroupBy(item => item.ParametersAsKey)
                    .Select(g => g.First().Transition)
                    .ToArray();

                WriteInternalTransitionMethodsForState(context, uniqueTransitions, state);

                WriteInboundTransitionMethodsForState(context, uniqueTransitions, state);
            }
        }

        private void WriteInboundTransitionMethodsForState(WriteContext context, StateTransition[] uniqueTransitions, string state)
        {
            var inboundTransitions = uniqueTransitions
                .Where(t => t.From != t.To) // skip internal transitions.
                .Where(t => t.To == state)
                .ToArray();

            foreach (var transition in inboundTransitions)
            {
                var typedNamedParameters = ToTypedNamedVariables(transition.Parameters);

                var transitionMethodName = ToTransitionMethodName(transition);
                WriteComment(context, new[] {transition}, "Implement this method to handle the transition below:");
                context.Writer.WriteLine($"protected virtual {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters})");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                if (transition.IsAsync)
                {
                    context.Writer.WriteLine("return Task.CompletedTask;");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();
            }
        }

        private void WriteInternalTransitionMethodsForState(WriteContext context, StateTransition[] uniqueTransitions, string state)
        {
            var internalTransitions = uniqueTransitions
                .Where(t => t.From == t.To) // only internal transitions.
                .Where(t => t.To == state)
                .ToArray();

            foreach (var transition in internalTransitions)
            {
                var transitionMethodName = ToTransitionMethodName(transition);
                var typedNamedParameters1 = transition.Parameters.Any() ? $"{ToTypedNamedVariables(transition.Parameters)}, " : string.Empty;
                var typedNamedParameters2 = ToTypedNamedVariables(transition.Parameters);
                var namedParameters = ToNamedVariables(transition.Parameters);

                WriteComment(context, new[] {transition}, "Implement this method to handle the transition below:");
                context.Writer.WriteLine($"protected virtual {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters2})");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                if (transition.IsAsync)
                {
                    context.Writer.WriteLine("return Task.CompletedTask;");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine(
                    $"private {(transition.IsAsync ? "Task" : "void")} {transitionMethodName}({typedNamedParameters1}{StateMachineType}.Transition transition) => {transitionMethodName}({namedParameters});");
                context.Writer.WriteLine();
            }
        }

        private void WriteComment(WriteContext context, StateTransition[] transitionSet, string message)
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
