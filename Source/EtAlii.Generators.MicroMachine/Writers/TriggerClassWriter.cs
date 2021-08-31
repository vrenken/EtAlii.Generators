namespace EtAlii.Generators.MicroMachine
{
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class TriggerClassWriter
    {
        private readonly TransitionConverter _transitionConverter;
        private readonly ParameterConverter _parameterConverter;

        public TriggerClassWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter)
        {
            _transitionConverter = transitionConverter;
            _parameterConverter = parameterConverter;
        }

        public void WriteTriggerClasses(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The classes below represent the triggers as used by the methods.");
            context.Writer.WriteLine();

            var triggers = StateFragment
                .GetAllTriggers(context.Instance.StateFragments);

            var baseClassName = "Trigger";
            WriteBaseClass(baseClassName, context);

            foreach (var trigger in triggers)
            {
                var className = $"{trigger}Trigger";
                WriteClass(className, baseClassName, context, trigger);
            }
        }

        private void WriteClass(string className, string baseClassName, WriteContext<StateMachine> context, string trigger)
        {
            var transitions = StateFragment.GetAllTransitions(context.Instance.StateFragments);

            var transitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(transitions, trigger);
            var transitionSet = transitionSets.First();
            var firstTransition = transitionSet.First();

            var parameters = firstTransition.Parameters;
            var typedParameters = _parameterConverter.ToTypedNamedVariables(parameters);
            var genericParameters = _parameterConverter.ToGenericParameters(parameters);
            var namedParameters = parameters.Any() ? $", {_parameterConverter.ToNamedVariables(parameters)}" : string.Empty;

            context.Writer.WriteLine(baseClassName != null ? $"protected class {className} : {baseClassName}" : $"protected class {className}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            foreach (var parameter in parameters)
            {
                var parameterName = _parameterConverter.ToParameterName(parameter);
                var camelCaseParameterName = _parameterConverter.ToCamelCase(parameterName);
                context.Writer.WriteLine($"public {parameter.Type} {camelCaseParameterName} {{get; private set;}}");
            }
            context.Writer.WriteLine("public {className}({typedParameters})");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            foreach (var parameter in parameters)
            {
                var parameterName = _parameterConverter.ToParameterName(parameter);
                var camelCaseParameterName = _parameterConverter.ToCamelCase(parameterName);
                context.Writer.WriteLine($"this.{camelCaseParameterName} = {parameterName}");
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }

        private void WriteBaseClass(string className, WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine($"protected abstract class {className}");
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
