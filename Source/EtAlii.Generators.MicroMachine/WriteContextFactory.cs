namespace EtAlii.Generators.MicroMachine
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class WriteContextFactory : IWriteContextFactory<StateMachine>
    {
        private readonly StateFragmentHelper _stateFragmentHelper;

        public WriteContextFactory(StateFragmentHelper stateFragmentHelper)
        {
            _stateFragmentHelper = stateFragmentHelper;
        }

        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext<StateMachine> Create(IndentedTextWriter writer, string originalFileName, List<string> log, StateMachine stateMachine)
        {
            var allTransitions = _stateFragmentHelper.GetAllTransitions(stateMachine.StateFragments);
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

            // We want to dump all unique states defined in the diagram.
            var allStates = _stateFragmentHelper.GetAllStates(stateMachine.StateFragments);
            log.Add("States found:");
            log.AddRange(allStates.Select(s => $"- {s}"));

            // We want to also dump all unique triggers defined in the diagram.
            var allTriggers = _stateFragmentHelper.GetAllTriggers(stateMachine.StateFragments);
            log.Add("Triggers found:");
            log.AddRange(allTriggers.Select(t => $"- {t}"));

            var usings = new[] {"System", "System.Threading.Tasks", "System.Collections.Generic" }.Concat(stateMachine.Usings).ToArray();
            var namespaceDetails = new NamespaceDetails(stateMachine.Namespace, usings);
            return new WriteContext(writer, originalFileName, stateMachine, namespaceDetails);
        }
    }
}
