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
                _log.Information("Writing default entry methods for {StateName}: OnEntry async = {OnEntryAsync}, OnExit async = {OnExitAsync}", state.Name, state.HasOnlyAsyncInboundTransitions, state.HasOnlyAsyncOutboundTransitions);
                var entryCall = new MethodCall(state, true, state.HasOnlyAsyncInboundTransitions);
                var exitCall = new MethodCall(state, true, state.HasOnlyAsyncOutboundTransitions);
                WriteEntryMethod(context, null, entryCall, writtenMethods);
                WriteExitMethod(context, null, exitCall, writtenMethods);
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
                            WriteExitMethod(context, trigger, exitCall, writtenMethods);
                        }

                        foreach (var entryCall in methodChain.EntryCalls)
                        {
                            WriteEntryMethod(context, trigger, entryCall, writtenMethods);
                        }
                    }
                }
            }
        }

        private void WriteExitMethod(WriteContext<StateMachine> context, string trigger, MethodCall methodCall, List<string> writtenMethods)
        {
            var writeAsyncExitMethod = methodCall.State.HasOnlyAsyncOutboundTransitions;
            var exitMethodName = $"On{methodCall.State.Name}Exited";
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
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{methodCall.State.Name}' state.");
            }
            else
            {
                context.Writer.WriteLine($"/// Implement this method to handle the exit of the '{methodCall.State.Name}' state by the '{trigger}' trigger.");
            }

            var writeAsync = methodCall.IsAsync;
            if (writeAsync)
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

        private void WriteEntryMethod(WriteContext<StateMachine> context, string trigger, MethodCall methodCall, List<string> writtenMethods)
        {
            var entryMethodName = $"On{methodCall.State.Name}Entered";
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
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{methodCall.State.Name}' state.");
            }
            else
            {
                context.Writer.WriteLine($"/// Implement this method to handle the entry of the '{methodCall.State.Name}' state by the '{trigger}' trigger.");
            }

            var writeAsync = methodCall.IsAsync;
            if (writeAsync)
            {
                context.Writer.WriteLine("/// <remark>");
                context.Writer.WriteLine("/// This method is configured to return a task because all transitions are marked to be called asynchronous.");
                context.Writer.WriteLine("/// </remark>");
            }

            context.Writer.WriteLine("/// </summary>");


            var choices = context.Instance.GenerateTriggerChoices
                ? $", {methodCall.State.Name}Choices choices"
                : "";

            if (context.Instance.GeneratePartialClass)
            {
                context.Writer.WriteLine($"private partial {(writeAsync ? "Task" : "void")} {entryMethodName}({triggerName} trigger{choices});");
            }
            else
            {
                context.Writer.WriteLine($"protected virtual {(writeAsync ? "Task" : "void")} {entryMethodName}({triggerName} trigger{choices})");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                if (writeAsync)
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
