namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class EventArgsWriter
    {
        private readonly MethodWriter _methodWriter;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public EventArgsWriter(MethodWriter methodWriter, StateFragmentHelper stateFragmentHelper)
        {
            _methodWriter = methodWriter;
            _stateFragmentHelper = stateFragmentHelper;
        }

        public void WriteEventArgs(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The classes below represent the EventArgs as used by some of the methods.");
            context.Writer.WriteLine();

            if(context.Instance.GenerateTriggerChoices)
            {
                var allStates = _stateFragmentHelper
                    .GetAllStates(context.Instance.StateFragments)
                    .ToArray();

                foreach (var state in allStates)
                {
                    context.Writer.WriteLine($"protected class {state}EventArgs");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    context.Writer.WriteLine($"private readonly {context.Instance.ClassName} _stateMachine;");
                    context.Writer.WriteLine();
                    context.Writer.WriteLine($"public {state}EventArgs({context.Instance.ClassName} stateMachine)");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    context.Writer.WriteLine($"_stateMachine = stateMachine;");
                    context.Writer.Indent -= 1;
                    context.Writer.WriteLine("}");
                    context.Writer.WriteLine("");
                    WriteMethods(context, state);
                    context.Writer.Indent -= 1;
                    context.Writer.WriteLine("}");
                    context.Writer.WriteLine();
                }
            }
        }

        private void WriteMethods(WriteContext<StateMachine> context, string state)
        {
            var outboundTransitions = _stateFragmentHelper.GetOutboundTransitions(context.Instance, state);

            foreach (var outboundTransition in outboundTransitions)
            {
                var transitionSets = new [] { new [] { outboundTransition } };
                if (outboundTransition.IsAsync)
                {
                    var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) =>
                    {
                        if (namedParameters.Any())
                        {
                            namedParameters = namedParameters.Substring(2);
                        }
                        return $"public Task {triggerName}Async({typedParameters}) => _stateMachine.{triggerName}Async({namedParameters});";
                    });
                    _methodWriter.WriteTriggerMethods(context, transitionSets, "async", asyncWrite);
                }
                else
                {
                    var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) =>
                    {
                        if (namedParameters.Any())
                        {
                            namedParameters = namedParameters.Substring(2);
                        }
                        return $"public void {triggerName}({typedParameters}) => _stateMachine.{triggerName}({namedParameters});";
                    });
                    _methodWriter.WriteTriggerMethods(context, transitionSets, "sync", syncWrite);
                }
            }
        }
    }
}
