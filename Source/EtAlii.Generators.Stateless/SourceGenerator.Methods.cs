namespace EtAlii.Generators.Stateless
{
    using System.Linq;

    public partial class SourceGenerator
    {
        /// <summary>
        /// Write the trigger methods through which the individual triggers can be fired.
        /// </summary>
        private void WriteTriggerMethods(WriteContext context)
        {
            context.Writer.WriteLine("// The methods below can be each called to fire a specific trigger");
            context.Writer.WriteLine("// and cause the state machine to transition to another state.");
            context.Writer.WriteLine();
            foreach (var trigger in context.AllTriggers)
            {
                var syncTransitions = context.UniqueParameterTransitions
                    .Where(t => t.Trigger == trigger)
                    .Where(t => !t.IsAsync)
                    .ToArray();
                if (syncTransitions.Any())
                {
                    foreach (var transition in syncTransitions)
                    {
                        var parameters = transition.Parameters;

                        var typedParameters = ToTypedNamedVariables(parameters);
                        var genericParameters = ToGenericParameters(parameters);
                        var namedParameters = transition.Parameters.Any() ? $", {ToNamedVariables(parameters)}" : string.Empty;
                        var triggerParameter = ToTriggerParameter(transition);

                        WriteComment(context, transition, "Call this method to trigger the transition below:");
                        context.Writer.WriteLine($"public void {trigger}({typedParameters}) => _stateMachine.Fire{genericParameters}({triggerParameter}{namedParameters});");
                        context.Writer.WriteLine();
                    }
                }

                var asyncTransitions = context.UniqueParameterTransitions
                    .Where(t => t.Trigger == trigger)
                    .Where(t => t.IsAsync)
                    .ToArray();
                if (asyncTransitions.Any())
                {
                    foreach (var transition in asyncTransitions)
                    {
                        var parameters = transition.Parameters;

                        var typedParameters = ToTypedNamedVariables(transition.Parameters);
                        var genericParameters = ToGenericParameters(transition.Parameters);
                        var namedParameters = transition.Parameters.Any() ? $", {ToNamedVariables(transition.Parameters)}" : string.Empty;
                        var triggerParameter = ToTriggerParameter(transition);

                        WriteComment(context, transition, "Call this method to trigger the transition below:");
                        context.Writer.WriteLine($"public Task {trigger}Async({typedParameters}) => _stateMachine.FireAsync{genericParameters}({triggerParameter}{namedParameters});");
                        context.Writer.WriteLine();
                    }
                }
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

                var uniqueTransitions = context.StateMachine.StateFragments
                    .OfType<StateTransition>()
                    .Select(t => new { Transition = t, ParametersAsKey = $"{t.To}{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                    .GroupBy(item => item.ParametersAsKey)
                    .Select(g => g.First().Transition)
                    .ToArray();

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

                    WriteComment(context, transition, "Implement this method to handle the transition below:");
                    context.Writer.WriteLine($"protected virtual {(transition.IsAsync ? "Task" : "void" )} {transitionMethodName}({typedNamedParameters2})");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    if (transition.IsAsync)
                    {
                        context.Writer.WriteLine("return Task.CompletedTask;");
                    }
                    context.Writer.Indent -= 1;
                    context.Writer.WriteLine("}");
                    context.Writer.WriteLine($"private {(transition.IsAsync ? "Task" : "void" )} {transitionMethodName}({typedNamedParameters1}{StateMachineType}.Transition transition) => {transitionMethodName}({namedParameters});");
                    context.Writer.WriteLine();
                }

                var inboundTransitions = uniqueTransitions
                    .Where(t => t.From != t.To) // skip internal transitions.
                    .Where(t => t.To == state)
                    .ToArray();

                foreach (var transition in inboundTransitions)
                {
                    var typedNamedParameters = ToTypedNamedVariables(transition.Parameters);

                    var transitionMethodName = ToTransitionMethodName(transition);
                    WriteComment(context, transition, "Implement this method to handle the transition below:");
                    context.Writer.WriteLine($"protected virtual {(transition.IsAsync ? "Task" : "void" )} {transitionMethodName}({typedNamedParameters})");
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
        }

        private void WriteComment(WriteContext context, StateTransition transition, string message)
        {
            context.Writer.WriteLine($"/// <summary>");
            context.Writer.WriteLine($"/// {message}<br/>");
            context.Writer.WriteLine($"/// {transition.From} --&gt; {transition.To} : {transition.Trigger}");
            context.Writer.WriteLine($"/// </summary>");
        }
    }
}
