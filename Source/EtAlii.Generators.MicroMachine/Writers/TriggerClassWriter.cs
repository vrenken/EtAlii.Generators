namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class TriggerClassWriter
    {
        private readonly MethodWriter _methodWriter;

        public TriggerClassWriter(MethodWriter methodWriter)
        {
            _methodWriter = methodWriter;
        }

        public void WriteTriggerClasses(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The classes below represent the triggers as used by the methods.");
            context.Writer.WriteLine();

            var triggers = StateFragment
                .GetAllTriggers(context.Instance.StateFragments);

            var baseClassName = "Trigger";
            WriteClass(baseClassName, null, context);

            foreach (var trigger in triggers)
            {
                var className = $"{trigger}Trigger";
                WriteClass(className, baseClassName, context);
            }
        }

        private void WriteClass(string className, string baseClassName, WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine(baseClassName != null ? $"protected class {className} : {baseClassName}" : $"protected class {className}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }


    //     private void WriteMethods(WriteContext<StateMachine> context, SuperState choiceSuperState)
    //     {
    //         var outboundTransitions = StateFragment.GetOutboundTransitions(context.Instance, choiceSuperState.Name);
    //
    //         foreach (var outboundTransition in outboundTransitions)
    //         {
    //             var transitionSets = new [] { new [] { outboundTransition } };
    //             if (outboundTransition.IsAsync)
    //             {
    //                 var asyncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) =>
    //                 {
    //                     if (namedParameters.Any())
    //                     {
    //                         namedParameters = namedParameters.Substring(2);
    //                     }
    //                     return $"public Task {triggerName}Async({typedParameters}) => _stateMachine.{triggerName}Async({namedParameters});";
    //                 });
    //                 _methodWriter.WriteTriggerMethods(context, transitionSets, "async", asyncWrite);
    //             }
    //             else
    //             {
    //                 var syncWrite = new Func<string, string, string, string, string, string>((triggerName, typedParameters, _, _, namedParameters) =>
    //                 {
    //                     if (namedParameters.Any())
    //                     {
    //                         namedParameters = namedParameters.Substring(2);
    //                     }
    //                     return $"public void {triggerName}({typedParameters}) => _stateMachine.{triggerName}({namedParameters});";
    //                 });
    //                 _methodWriter.WriteTriggerMethods(context, transitionSets, "sync", syncWrite);
    //             }
    //         }
    //     }
    }
}
