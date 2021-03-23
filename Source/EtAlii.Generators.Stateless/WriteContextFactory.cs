namespace EtAlii.Generators.Stateless
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;

    public class WriteContextFactory
    {
        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext Create(IndentedTextWriter writer, string originalFileName, List<string> log, StateMachine stateMachine)
        {
            var allTransitions = stateMachine.StateFragments
                .OfType<Transition>()
                .ToArray();

            log.Add("Transitions found:");
            log.AddRange(allTransitions.Select(t =>
            {
                var parameters = t.Parameters.Any()
                    ? $" ({string.Join(", ", t.Parameters.Select(p => $"{p.Type} {p.Name}".Trim()))}) "
                    : " ";

                var asyncPrefix = t.IsAsync ? "async " : "";
                var stereoType = t.IsAsync || t.Parameters.Any()
                    ? $" << {asyncPrefix}{parameters}>> "
                    : " ";
                return $"- {t.From} -> {t.To}{stereoType}: {t.Trigger}";
            }));

            // We want to know all unique states defined in the diagram.
            var transitionStates = allTransitions.SelectMany(t => new[] { t.From, t.To });
            var descriptionStates = stateMachine.StateFragments.OfType<StateDescription>().SelectMany(t => new[] { t.State });
            var allStates = transitionStates
                .Concat(descriptionStates)
                .Where(s => s != null)
                .OrderBy(s => s)
                .Distinct() // That is, of course without any doubles.
                .ToArray();

            log.Add("States found:");
            log.AddRange(allStates.Select(s => $"- {s}"));

            // We want to know all unique triggers defined in the diagram.
            var transitionTriggers = allTransitions.Select(t => t.Trigger);
            var allTriggers = transitionTriggers
                .OrderBy(t => t)
                .Distinct() // That is, of course without any doubles.
                .ToArray();

            log.Add("Triggers found:");
            log.AddRange(allTriggers.Select(t => $"- {t}"));

            // We want to know all unique transitions defined in the diagram.
            // That is, the transitions grouped by the trigger and unique sequence of parameters.
            var uniqueParameterTransitions = allTransitions
                .Select(t => new { Transition = t, ParametersAsKey = $"{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.First().Transition)
                .ToArray();

            return new WriteContext(writer, originalFileName, stateMachine, allStates, allTriggers, allTransitions, uniqueParameterTransitions);
        }
    }
}
