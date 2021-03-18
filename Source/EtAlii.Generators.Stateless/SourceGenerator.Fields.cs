namespace EtAlii.Generators.Stateless
{
    using System.Linq;

    public partial class SourceGenerator
    {
        /// <summary>
        /// Write the trigger members for all transitions that have parameters.
        /// </summary>
        private void WriteAllTriggerMembers(WriteContext context)
        {
            // We only need to write a trigger member for all relations that have parameters.
            var uniqueTransitions = context.UniqueParameterTransitions
                .Where(t => t.Parameters.Any())
                .ToArray();

            foreach (var transition in uniqueTransitions)
            {
                var genericParameters = ToGenericParameters(transition.Parameters);
                var triggerMemberName = ToTriggerMemberName(transition);
                var triggerType = transition.Parameters.Any()
                    ? "TriggerWithParameters"
                    : "Trigger";
                context.Writer.WriteLine($"private StateMachine<State, Trigger>.{triggerType}{genericParameters} {triggerMemberName};");
            }
        }
    }
}
