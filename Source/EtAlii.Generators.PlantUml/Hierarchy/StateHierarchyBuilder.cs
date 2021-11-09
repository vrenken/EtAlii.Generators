namespace EtAlii.Generators.PlantUml
{
    using System.Collections.Generic;
    using System.Linq;

    public class StateHierarchyBuilder
    {
        private readonly StateFragmentHelper _stateFragmentHelper;

        public StateHierarchyBuilder(StateFragmentHelper stateFragmentHelper)
        {
            _stateFragmentHelper = stateFragmentHelper;
        }

        public (State[] hierarchicalStates, State[] sequentialStates) Build(StateFragment[] fragments)
        {
            var allStates = _stateFragmentHelper.GetAllStates(fragments);

            var sequentialStates = new List<State>();

            var rootStates = allStates
                .Where(s => _stateFragmentHelper.GetSuperState(fragments, s) == null)
                .Select(s => Build(fragments, sequentialStates, s))
                .ToArray();

            // Weird patch. No idea why this is needed.
            sequentialStates = sequentialStates
                .GroupBy(s => s.Name)
                .Select(g => g.First())
                .ToList();

            return (rootStates, sequentialStates.ToArray());
        }

        private State Build(StateFragment[] fragments, List<State> sequentialStates, string stateName)
        {
            var length = sequentialStates.Count;
            var allTransitions = _stateFragmentHelper
                .GetAllTransitions(fragments)
                .Where(t => t.From == stateName)
                .ToArray();

            var state = new State
            {
                Name = stateName,
                AllTransitions = allTransitions,
                InboundTransitions = _stateFragmentHelper.GetInboundTransitions(fragments, stateName),
                OutboundTransitions = _stateFragmentHelper.GetOutboundTransitions(fragments, stateName),
                InternalTransitions = allTransitions
                    .Where(t => t.To == stateName && t.To == t.From)
                    .ToArray(),

                Children = _stateFragmentHelper
                    .GetAllSubStates(fragments, stateName)
                    .Select(c => Build(fragments, sequentialStates, c))
                    .ToArray(),
                HasOnlyAsyncOutboundTransitions = _stateFragmentHelper.HasOnlyAsyncOutboundTransitions(fragments, stateName),
                HasOnlyAsyncInboundTransitions = _stateFragmentHelper.HasOnlyAsyncInboundTransitions(fragments, stateName),
            };
            sequentialStates.Insert(length, state);
            return state;
        }
    }
}
