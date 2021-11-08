namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class MethodWriter
    {
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly ILogger _log = Log.ForContext<MethodWriter>();

        public MethodWriter(StateFragmentHelper stateFragmentHelper)
        {
            _stateFragmentHelper = stateFragmentHelper;
        }

        /// <summary>
        /// Write the transition methods through which behavior can be implemented for the state transitions.
        /// </summary>
        /// <param name="context"></param>
        public void WriteTransitionMethods(WriteContext<StateMachine> context)
        {
            _log.Information("Writing transition methods for {ClassName}", context.Instance.ClassName);

            var writtenMethods = new List<string>();

            var allStates = _stateFragmentHelper.GetAllStates(context.Instance.StateFragments);
            foreach (var state in allStates)
            {
                // var isChoiceState = StateFragment.GetAllSuperStates(context.Instance.StateFragments)
                //     .Any(ss egt ss.Name eq state and ss.StereoType eq StereoType.Choice)

                var writeAsyncEntryMethod = _stateFragmentHelper.HasOnlyAsyncInboundTransitions(context.Instance, state);
                var writeAsyncExitMethod = _stateFragmentHelper.HasOnlyAsyncOutboundTransitions(context.Instance, state);

                WriteEntryMethod(context, state, null, writeAsyncEntryMethod, writtenMethods);
                WriteExitMethod(context, state, null, writeAsyncExitMethod, writtenMethods);

                var inboundTransitions = _stateFragmentHelper.GetInboundTransitions(context.Instance.StateFragments, state);
                foreach (var inboundTransition in inboundTransitions)
                {
                    WriteEntryMethod(context, state, inboundTransition.Trigger, writeAsyncEntryMethod, writtenMethods);
                }
                var outboundTransitions = _stateFragmentHelper.GetOutboundTransitions(context.Instance, state);
                foreach (var outboundTransition in outboundTransitions)
                {
                    WriteExitMethod(context, state, outboundTransition.Trigger, writeAsyncExitMethod, writtenMethods);
                }
                var internalTransitions = _stateFragmentHelper.GetInternalTransitions(context.Instance.StateFragments, state);
                foreach (var internalTransition in internalTransitions)
                {
                    WriteEntryMethod(context, state, internalTransition.Trigger, writeAsyncEntryMethod, writtenMethods);
                    WriteExitMethod(context, state, internalTransition.Trigger, writeAsyncExitMethod, writtenMethods);
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
