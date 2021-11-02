namespace EtAlii.Generators.Stateless
{
    using System;
    using System.CodeDom.Compiler;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class WriteContextFactory : IWriteContextFactory<StateMachine>
    {
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly ILogger _logger = Log.ForContext<WriteContextFactory>();

        public WriteContextFactory(StateFragmentHelper stateFragmentHelper)
        {
            _stateFragmentHelper = stateFragmentHelper;
        }

        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext<StateMachine> Create(IndentedTextWriter writer, string originalFileName, StateMachine stateMachine)
        {
            var allTransitions = _stateFragmentHelper.GetAllTransitions(stateMachine.StateFragments);
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
            }), "");
            _logger
                .ForContext("Transitions", transitionsAsText)
                .Information("Found {TransitionCount} transitions", allTransitions.Length);

            // We want to dump all unique states defined in the diagram.
            var allStates = _stateFragmentHelper.GetAllStates(stateMachine.StateFragments);
            var statesAsText = string.Join(Environment.NewLine, allStates.Select(s => $"- {s}"));
            _logger
                .ForContext("States", statesAsText)
                .Information("Found {StateCount} states", allStates.Length);

            // We want to also dump all unique triggers defined in the diagram.
            var allTriggers = _stateFragmentHelper.GetAllTriggers(stateMachine.StateFragments);
            var triggersAsText = string.Join(Environment.NewLine, allTriggers.Select(t => $"- {t}"));
            _logger
                .ForContext("Triggers", triggersAsText)
                .Information("Found {TriggerCount} triggers", allTriggers.Length);

            var usings = new[] {"System", "System.Threading.Tasks", "Stateless",}.Concat(stateMachine.Usings).ToArray();
            var namespaceDetails = new NamespaceDetails(stateMachine.Namespace, usings);
            return new WriteContext(writer, originalFileName, stateMachine, namespaceDetails);
        }
    }
}
