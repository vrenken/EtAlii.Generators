namespace EtAlii.Generators.Stateless
{
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class FieldWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public FieldWriter(
            ParameterConverter parameterConverter,
            TransitionConverter transitionConverter, StateFragmentHelper stateFragmentHelper)
        {
            _parameterConverter = parameterConverter;
            _transitionConverter = transitionConverter;
            _stateFragmentHelper = stateFragmentHelper;
        }

        /// <summary>
        /// Write the trigger members for all transitions that have parameters.
        /// </summary>
        public void WriteAllTriggerFields(WriteContext<StateMachine> context)
        {
            // We only need to write a trigger member for all relations that have parameters.
            var uniqueTransitionsWithParameters = _stateFragmentHelper.GetUniqueParameterTransitions(context.Instance)
                .Where(t => t.Parameters.Any())
                .ToArray();

            foreach (var transition in uniqueTransitionsWithParameters)
            {
                var genericParameters = _parameterConverter.ToGenericParameters(transition.Parameters);
                var triggerMemberName = _transitionConverter.ToTriggerMemberName(transition);
                var triggerType = transition.Parameters.Any()
                    ? "TriggerWithParameters"
                    : "Trigger";
                context.Writer.WriteLine($"private {StatelessWriter.StateMachineType}.{triggerType}{genericParameters} {triggerMemberName};");
            }
        }
    }
}
