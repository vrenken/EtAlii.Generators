namespace EtAlii.Generators.MicroMachine
{
    using System;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class ChoicesWriter
    {
        private readonly MethodWriter _methodWriter;
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly ILogger _log = Log.ForContext<ChoicesWriter>();

        public ChoicesWriter(MethodWriter methodWriter, StateFragmentHelper stateFragmentHelper)
        {
            _methodWriter = methodWriter;
            _stateFragmentHelper = stateFragmentHelper;
        }

        public void WriteChoices(WriteContext<StateMachine> context)
        {
            _log.Information("Writing choice classes for {ClassName}", context.Instance.ClassName);

            if(context.Instance.GenerateTriggerChoices)
            {
                context.Writer.WriteLine("// The classes below represent the choices as available to transition from one state to the other.");
                context.Writer.WriteLine();

                var states = _stateFragmentHelper
                    .GetAllStates(context.Instance.StateFragments)
                    .ToArray();

                foreach (var state in states)
                {
                    context.Writer.WriteLine($"protected class {state}Choices");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    context.Writer.WriteLine($"private readonly {context.Instance.ClassName} _stateMachine;");
                    context.Writer.WriteLine();
                    context.Writer.WriteLine($"public {state}Choices({context.Instance.ClassName} stateMachine)");
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
                    var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => _stateMachine.{triggerName}Async({namedParameters});");
                    _methodWriter.WriteTriggerMethods(context, transitionSets, "async", asyncWrite);
                }
                else
                {
                    var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public void {triggerName}({typedParameters}) => _stateMachine.{triggerName}({namedParameters});");
                    _methodWriter.WriteTriggerMethods(context, transitionSets, "sync", syncWrite);
                }
            }

            var internalTransitions = _stateFragmentHelper.GetInternalTransitions(context.Instance.StateFragments, state);

            foreach (var internalTransition in internalTransitions)
            {
                var transitionSets = new [] { new [] { internalTransition } };
                if (internalTransition.IsAsync)
                {
                    var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => _stateMachine.{triggerName}Async({namedParameters});");
                    _methodWriter.WriteTriggerMethods(context, transitionSets, "async", asyncWrite);
                }
                else
                {
                    var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public void {triggerName}({typedParameters}) => _stateMachine.{triggerName}({namedParameters});");
                    _methodWriter.WriteTriggerMethods(context, transitionSets, "sync", syncWrite);
                }
            }

        }
    }
}
