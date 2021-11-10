namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class TransitionMethodWriter
    {
        private readonly MethodChainBuilder _methodChainBuilder;
        private readonly ILogger _log = Log.ForContext<TransitionMethodWriter>();

        public TransitionMethodWriter(MethodChainBuilder methodChainBuilder)
        {
            _methodChainBuilder = methodChainBuilder;
        }

        /// <summary>
        /// Write the transition methods through which behavior can be implemented for the state transitions.
        /// </summary>
        /// <param name="context"></param>
        public void WriteTransitionMethods(WriteContext<StateMachine> context)
        {
            _log.Information("Writing transition methods for {ClassName}", context.Instance.ClassName);

            var writtenMethods = new List<string>();

            foreach (var state in context.Instance.SequentialStates)
            {
                var writeAsyncEntryMethod = state.HasOnlyAsyncInboundTransitions;
                var writeAsyncExitMethod = state.HasOnlyAsyncOutboundTransitions;

                WriteExitMethod(context, state, null, writeAsyncExitMethod, writtenMethods);
                WriteEntryMethod(context, state, null, writeAsyncEntryMethod, writtenMethods);
            }
            WriteMethodsBasedOnTriggers(context, writtenMethods);
        }

        private void WriteMethodsBasedOnTriggers(WriteContext<StateMachine> context, List<string> writtenMethods)
        {
            foreach (var trigger in context.Instance.AllTriggers)
            {
                var transitions = context.Instance.AllTransitions
                    .Where(t => t.Trigger == trigger)
                    .ToArray();

                foreach (var transition in transitions)
                {
                    var methodChains = _methodChainBuilder.Build(context.Instance, transition);
                    foreach (var methodChain in methodChains)
                    {
                        foreach (var exitCall in methodChain.ExitCalls)
                        {
                            var fromState = context.Instance.SequentialStates.Single(s => s.Name == exitCall.State.Name);
                            var writeAsyncExitMethod = fromState.HasOnlyAsyncOutboundTransitions;
                            WriteExitMethod(context, fromState, trigger, writeAsyncExitMethod, writtenMethods);
                        }

                        foreach (var entryCall in methodChain.EntryCalls)
                        {
                            var toState = context.Instance.SequentialStates.Single(s => s.Name == entryCall.State.Name);
                            var writeAsyncEntryMethod = toState.HasOnlyAsyncInboundTransitions;
                            WriteEntryMethod(context, toState, trigger, writeAsyncEntryMethod, writtenMethods);
                        }
                    }
                }
            }
        }

        private void WriteExitMethod(WriteContext<StateMachine> context, State state, string trigger, bool writeAsyncEntryMethod, List<string> writtenMethods)
        {
            var writeAsyncExitMethod = state.HasOnlyAsyncOutboundTransitions;
            var exitMethodName = $"On{state.Name}Exited";
            var triggerName = trigger == null ? "Trigger" : $"{trigger}Trigger";

            var key = $"{exitMethodName}({triggerName} trigger)";
            if (writtenMethods.Contains(key))
            {
                return;
            }
            writtenMethods.Add(key);

            context.Writer.WriteLine("/// <summary>");
            if (trigger == null)
            {
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{state.Name}' state.");
            }
            else
            {
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{state.Name}' state by the '{trigger}' trigger.");
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
                context.Writer.WriteLine($"private partial {(writeAsyncExitMethod ? "Task" : "void")} {exitMethodName}({triggerName} trigger);");
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

        private void WriteEntryMethod(WriteContext<StateMachine> context, State state, string trigger, bool writeAsyncEntryMethod, List<string> writtenMethods)
        {
            var entryMethodName = $"On{state.Name}Entered";
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
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{state.Name}' state.");
            }
            else
            {
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{state.Name}' state by the '{trigger}' trigger.");
            }
            if (writeAsyncEntryMethod)
            {
                context.Writer.WriteLine("/// <remark>");
                context.Writer.WriteLine("/// This method is configured to return a task because all transitions are marked to be called asynchronous.");
                context.Writer.WriteLine("/// </remark>");
            }

            context.Writer.WriteLine("/// </summary>");


            var choices = context.Instance.GenerateTriggerChoices
                ? $", {state.Name}Choices choices"
                : "";

            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"private partial {(writeAsyncEntryMethod ? "Task" : "void")} {entryMethodName}({triggerName} trigger{choices});");
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
    }
}
