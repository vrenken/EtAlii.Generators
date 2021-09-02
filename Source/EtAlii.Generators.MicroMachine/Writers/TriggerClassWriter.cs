namespace EtAlii.Generators.MicroMachine
{
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class TriggerClassWriter
    {
        private readonly TransitionConverter _transitionConverter;
        private readonly ParameterConverter _parameterConverter;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public TriggerClassWriter(ParameterConverter parameterConverter, TransitionConverter transitionConverter, StateFragmentHelper stateFragmentHelper)
        {
            _transitionConverter = transitionConverter;
            _stateFragmentHelper = stateFragmentHelper;
            _parameterConverter = parameterConverter;
        }

        public void WriteTriggerClasses(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The classes below represent the triggers as used by the methods.");
            context.Writer.WriteLine();

            var triggers = _stateFragmentHelper
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
            var transitions = _stateFragmentHelper.GetAllTransitions(context.Instance.StateFragments);

            var transitionSets = _transitionConverter.ToTransitionsSetsPerTriggerAndUniqueParameters(transitions, trigger);
            var transitionSet = transitionSets.First();
            var firstTransition = transitionSet.First();

            var parameters = firstTransition.Parameters;
            var typedParameters = _parameterConverter.ToTypedNamedVariables(parameters);

            context.Writer.WriteLine(baseClassName != null ? $"protected class {className} : {baseClassName}" : $"protected class {className}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            var properties = _parameterConverter.ToProperties(parameters);
            foreach (var property in properties)
            {
                context.Writer.WriteLine(property);
            }
            context.Writer.WriteLine();
            context.Writer.WriteLine($"public {className}({typedParameters})");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            var propertyAssignments = _parameterConverter.ToPropertyAssignments(parameters);
            foreach (var propertyAssignment in propertyAssignments)
            {
                context.Writer.WriteLine(propertyAssignment);
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
