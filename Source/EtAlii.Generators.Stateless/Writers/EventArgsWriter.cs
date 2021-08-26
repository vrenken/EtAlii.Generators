namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class EventArgsWriter
    {
        private readonly MethodWriter _methodWriter;
        private readonly TransitionConverter _transitionConverter;

        public EventArgsWriter(MethodWriter methodWriter, TransitionConverter transitionConverter)
        {
            _methodWriter = methodWriter;
            _transitionConverter = transitionConverter;
        }

        public void WriteEventArgs(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The classes below represent the EventArgs as used by some of the methods.");
            context.Writer.WriteLine();

            var choiceSuperStates = StateFragment
                .GetAllSuperStates(context.Instance.StateFragments)
                .Where(ss => ss.StereoType == StereoType.Choice);

            foreach (var choiceSuperState in choiceSuperStates)
            {
                context.Writer.WriteLine($"protected class {choiceSuperState.Name}EventArgs");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                context.Writer.WriteLine($"private readonly {context.Instance.ClassName} _stateMachine;");
                context.Writer.WriteLine();
                context.Writer.WriteLine($"public {choiceSuperState.Name}EventArgs({context.Instance.ClassName} stateMachine)");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;
                context.Writer.WriteLine($"_stateMachine = stateMachine;");
                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine("");
                WriteMethods(context, choiceSuperState);
                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();
            }
        }

        private void WriteMethods(WriteContext<StateMachine> context, SuperState choiceSuperState)
        {
            var outboundTransitions = StateFragment.GetOutboundTransitions(context.Instance, choiceSuperState.Name);

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
