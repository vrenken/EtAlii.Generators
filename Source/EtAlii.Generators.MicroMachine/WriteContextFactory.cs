namespace EtAlii.Generators.MicroMachine
{
    using System;
    using System.CodeDom.Compiler;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class WriteContextFactory : IWriteContextFactory<StateMachine>
    {
        private readonly ILogger _log = Log.ForContext<WriteContextFactory>();

        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext<StateMachine> Create(IndentedTextWriter writer, string originalFileName, StateMachine stateMachine)
        {
            var allTransitions = stateMachine.AllTransitions;
            var transitionsAsText = string.Join(Environment.NewLine, allTransitions.Select(t =>
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
            _log
                .ForContext("Transitions", transitionsAsText)
                .Information("Transitions found {TransitionCount}", allTransitions.Length);

            // We want to dump all unique states defined in the diagram.
            var allStates = stateMachine.SequentialStates
                .Select(s => s.Name)
                .ToArray();
            var statesAsText = string.Join(Environment.NewLine, allStates.Select(s => $"- {s}"));
            _log
                .ForContext("States", statesAsText)
                .Information("Found {StateCount} states", allStates.Length);

            // We want to also dump all unique triggers defined in the diagram.
            var allTriggers = stateMachine.AllTriggers;
            var triggersAsText = string.Join(Environment.NewLine, allTriggers.Select(t => $"- {t}"));
            _log
                .ForContext("Triggers", triggersAsText)
                .Information("Found {TriggerCount} triggers", allTriggers.Length);

            var usings = new[] {"System", "System.Threading.Tasks", "System.Collections.Generic" }.Concat(stateMachine.Usings).ToArray();
            var namespaceDetails = new NamespaceDetails(stateMachine.Namespace, usings);
            return new WriteContext(writer, originalFileName, stateMachine, namespaceDetails);
        }
    }
}
