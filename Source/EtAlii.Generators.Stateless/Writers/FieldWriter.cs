namespace EtAlii.Generators.Stateless
{
    using System.Linq;

    public class FieldWriter
    {
        private readonly ParameterConverter _parameterConverter;
        private readonly TransitionConverter _transitionConverter;
        public FieldWriter(
            ParameterConverter parameterConverter,
            TransitionConverter transitionConverter)
        {
            _parameterConverter = parameterConverter;
            _transitionConverter = transitionConverter;
        }

        /// <summary>
        /// Write the trigger members for all transitions that have parameters.
        /// </summary>
        public void WriteAllTriggerFields(WriteContext context)
        {
            // We only need to write a trigger member for all relations that have parameters.
            var uniqueTransitionsWithParameters = StateFragment.GetUniqueParameterTransitions(context.Instance.StateFragments)
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
