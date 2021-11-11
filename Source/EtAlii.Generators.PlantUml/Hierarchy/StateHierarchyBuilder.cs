namespace EtAlii.Generators.PlantUml
{
    using System.Collections.Generic;
    using System.Linq;
    using Serilog;

    public partial class StateHierarchyBuilder
    {
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly IStateMachineLifetime _lifetime;
        private readonly ILogger _log = Log.ForContext<StateHierarchyBuilder>();

        public StateHierarchyBuilder(StateFragmentHelper stateFragmentHelper, IStateMachineLifetime lifetime)
        {
            _stateFragmentHelper = stateFragmentHelper;
            _lifetime = lifetime;
        }

        public (State[] hierarchicalStates, State[] sequentialStates) Build(StateFragment[] fragments, SuperState[] allSuperStates, Transition[] allTransitions)
        {
            var allStates = GetAllStates(fragments);

            var sequentialStates = new List<State>();

            var rootStates = allStates
                .Where(s => GetParent(allSuperStates, s) == null)
                .Select(s => Build(fragments, sequentialStates, s, allSuperStates, allTransitions))
                .ToArray();

            // Weird patch. No idea why this is needed.
            sequentialStates = sequentialStates
                .GroupBy(s => s.Name)
                .Select(g => g.First())
                .ToList();

            return (rootStates, sequentialStates.ToArray());
        }

        private State Build(StateFragment[] fragments, List<State> sequentialStates, string stateName, SuperState[] allSuperStates, Transition[] allTransitions)
        {
            var length = sequentialStates.Count;

            var children = GetAllSubStates(stateName, allSuperStates)
                .Select(c => Build(fragments, sequentialStates, c, allSuperStates, allTransitions))
                .ToArray();
            var allChildren = GetAllChildren(children);

            var allSubStates = GetAllSubStates(stateName, allSuperStates);

            var parent = GetParent(allSuperStates, stateName);
            var allParents = GetAllParents(allSuperStates, stateName);

            var inboundTransitions = allTransitions
                .Where(t => t.To == stateName)
                .Where(t => t.From != t.To)
                .ToArray();
            var outboundTransitions = parent != null
                ? parent.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From == stateName)
                    .Where(t => t.To != stateName)
                    .ToArray()
                : fragments
                    .OfType<Transition>()
                    .Where(t => t.From == stateName)
                    .Where(t => t.To != stateName)
                    .ToArray();
            var internalTransitions = allTransitions
                .Where(t => t.To == stateName && t.To == t.From)
                .ToArray();

            var state = new State
            {
                Name = stateName,
                Parent = parent,
                AllParents = allParents,
                Children = children,
                AllChildren = allChildren,
                AllSubStates = allSubStates,

                AllTransitions = allTransitions,
                InboundTransitions = inboundTransitions,
                OutboundTransitions = outboundTransitions,
                InternalTransitions = internalTransitions,

                HasOnlyAsyncOutboundTransitions = HasOnlyAsyncOutboundTransitions(allTransitions, allSuperStates, outboundTransitions, stateName),
                HasOnlyAsyncInboundTransitions = HasOnlyAsyncInboundTransitions(allTransitions, allSuperStates, inboundTransitions, stateName),
            };
            sequentialStates.Insert(length, state);
            return state;
        }
    }
}
