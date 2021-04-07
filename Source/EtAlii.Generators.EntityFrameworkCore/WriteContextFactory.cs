namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;

    public class WriteContextFactory
    {
        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext Create(IndentedTextWriter writer, string originalFileName, List<string> log, EntityModel model)
        {
            // var allTransitions = StateFragment.GetAllTransitions(model.StateFragments);
            // log.Add("Transitions found:");
            // log.AddRange(allTransitions.Select(t =>
            // {
            //     var parameters = t.Parameters.Any()
            //         ? $" ({string.Join(", ", t.Parameters.Select(p => $"{p.Type} {p.Name}".Trim()))}) "
            //         : " ";
            //
            //     var asyncPrefix = t.IsAsync ? "async " : "";
            //     var stereoType = t.IsAsync || t.Parameters.Any()
            //         ? $" << {asyncPrefix}{parameters}>> "
            //         : " ";
            //     return $"- {t.From} -> {t.To}{stereoType}: {t.Trigger}";
            // }));
            //
            // // We want to dump all unique states defined in the diagram.
            // var allStates = StateFragment.GetAllStates(model.StateFragments);
            // log.Add("States found:");
            // log.AddRange(allStates.Select(s => $"- {s}"));
            //
            // // We want to also dump all unique triggers defined in the diagram.
            // var allTriggers = StateFragment.GetAllTriggers(model.StateFragments);
            // log.Add("Triggers found:");
            // log.AddRange(allTriggers.Select(t => $"- {t}"));

            return new WriteContext(writer, originalFileName, model);
        }
    }
}
