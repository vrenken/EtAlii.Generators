namespace EtAlii.Generators.MicroMachine
{
    using System;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class ChoicesWriter
    {
        private readonly TriggerMethodWriter _triggerMethodWriter;
        private readonly ILogger _log = Log.ForContext<ChoicesWriter>();

        public ChoicesWriter(TriggerMethodWriter triggerMethodWriter)
        {
            _triggerMethodWriter = triggerMethodWriter;
        }

        public void WriteChoices(WriteContext<StateMachine> context)
        {
            _log.Information("Writing choice classes for {ClassName}", context.Instance.ClassName);

            if(context.Instance.GenerateTriggerChoices)
            {
                context.Writer.WriteLine("// The classes below represent the choices as available to transition from one state to the other.");
                context.Writer.WriteLine();

                foreach (var state in context.Instance.SequentialStates)
                {
                    context.Writer.WriteLine($"protected class {state.Name}Choices");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;
                    context.Writer.WriteLine($"private readonly {context.Instance.ClassName} _stateMachine;");
                    context.Writer.WriteLine();
                    context.Writer.WriteLine($"public {state.Name}Choices({context.Instance.ClassName} stateMachine)");
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

        private void WriteMethods(WriteContext<StateMachine> context, State state)
        {
            foreach (var outboundTransition in state.OutboundTransitions)
            {
                var transitionSets = new [] { new [] { outboundTransition } };
                if (outboundTransition.IsAsync)
                {
                    var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => _stateMachine.{triggerName}Async({namedParameters});");
                    _triggerMethodWriter.WriteTriggerMethods(context, transitionSets, "async", asyncWrite);
                }
                else
                {
                    var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public void {triggerName}({typedParameters}) => _stateMachine.{triggerName}({namedParameters});");
                    _triggerMethodWriter.WriteTriggerMethods(context, transitionSets, "sync", syncWrite);
                }
            }

            foreach (var internalTransition in state.InternalTransitions)
            {
                var transitionSets = new [] { new [] { internalTransition } };
                if (internalTransition.IsAsync)
                {
                    var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public Task {triggerName}Async({typedParameters}) => _stateMachine.{triggerName}Async({namedParameters});");
                    _triggerMethodWriter.WriteTriggerMethods(context, transitionSets, "async", asyncWrite);
                }
                else
                {
                    var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) => $"public void {triggerName}({typedParameters}) => _stateMachine.{triggerName}({namedParameters});");
                    _triggerMethodWriter.WriteTriggerMethods(context, transitionSets, "sync", syncWrite);
                }
            }
        }
    }
}
