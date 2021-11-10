namespace EtAlii.Generators.PlantUml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StateHierarchyBuilder
    {
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly IStateMachineLifetime _lifetime;

        public StateHierarchyBuilder(StateFragmentHelper stateFragmentHelper, IStateMachineLifetime lifetime)
        {
            _stateFragmentHelper = stateFragmentHelper;
            _lifetime = lifetime;
        }

        public (State[] hierarchicalStates, State[] sequentialStates) Build(StateFragment[] fragments, SuperState[] allSuperStates)
        {
            var allStates = _stateFragmentHelper.GetAllStates(fragments);

            var sequentialStates = new List<State>();

            var rootStates = allStates
                .Where(s => _stateFragmentHelper.GetSuperState(allSuperStates, s) == null)
                .Select(s => Build(fragments, sequentialStates, s, allSuperStates))
                .ToArray();

            // Weird patch. No idea why this is needed.
            sequentialStates = sequentialStates
                .GroupBy(s => s.Name)
                .Select(g => g.First())
                .ToList();

            return (rootStates, sequentialStates.ToArray());
        }

        private State Build(StateFragment[] fragments, List<State> sequentialStates, string stateName, SuperState[] allSuperStates)
        {
            var length = sequentialStates.Count;
            var allTransitions = _stateFragmentHelper
                .GetAllTransitions(fragments)
                .Where(t => t.From == stateName)
                .ToArray();

            var children = GetAllSubStates(stateName, allSuperStates)
                .Select(c => Build(fragments, sequentialStates, c, allSuperStates))
                .ToArray();
            var allChildren = GetAllChildren(children);

            var inboundTransitions = GetInboundTransitions(allTransitions, stateName);
            var outboundTransitions = GetOutboundTransitions(fragments, allSuperStates, stateName);

            var state = new State
            {
                Name = stateName,
                AllTransitions = allTransitions,
                InboundTransitions = inboundTransitions,
                OutboundTransitions = outboundTransitions,
                InternalTransitions = allTransitions
                    .Where(t => t.To == stateName && t.To == t.From)
                    .ToArray(),

                Children = children,
                AllChildren = allChildren,
                HasOnlyAsyncOutboundTransitions = HasOnlyAsyncOutboundTransitions(allTransitions, allSuperStates, outboundTransitions, stateName),
                HasOnlyAsyncInboundTransitions = HasOnlyAsyncInboundTransitions(allTransitions, allSuperStates, inboundTransitions, stateName),
            };
            sequentialStates.Insert(length, state);
            return state;
        }

        private bool HasOnlyAsyncOutboundTransitions(Transition[] allTransitions, SuperState[] allSuperStates, Transition[] outboundTransitions, string state)
        {
            var superState = allSuperStates
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = _stateFragmentHelper.GetAllSubStates(superState);
                var directTransitionsFromSubState = allTransitions
                    .Where(t => t.To != state && !allSubstates.Contains(t.To) && allSubstates.Contains(t.From))
                    .ToArray();

                outboundTransitions = outboundTransitions
                    .Concat(directTransitionsFromSubState)
                    .ToArray();
            }

            return
                outboundTransitions.Any() &&
                outboundTransitions.All(t => t.IsAsync) &&
                state != _lifetime.BeginStateName &&
                state != _lifetime.EndStateName;
        }

        private bool HasOnlyAsyncInboundTransitions(Transition[] allTransitions, SuperState[] allSuperStates, Transition[] inboundTransitions, string state)
        {
            var superState = allSuperStates
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = _stateFragmentHelper.GetAllSubStates(superState);
                var directTransitionsToSubState = allTransitions
                    .Where(t => t.From != state && allSubstates.Contains(t.To) && !allSubstates.Contains(t.From))
                    .ToArray();

                inboundTransitions = inboundTransitions
                    .Concat(directTransitionsToSubState)
                    .ToArray();
            }

            return
                inboundTransitions.Any() &&
                inboundTransitions.All(t => t.IsAsync) &&
                state != _lifetime.BeginStateName &&
                state != _lifetime.EndStateName;
        }
        private State[] GetAllChildren(State[] children)
        {
            var result = new List<State>();

            result.AddRange(children);
            foreach (var childState in children)
            {
                var childChildren = GetAllChildren(childState.Children);
                result.AddRange(childChildren);
            }
            return result.ToArray();
        }

        private string[] GetAllSubStates(string stateName, SuperState[] allSuperStates)
        {
            var superState = allSuperStates
                .SingleOrDefault(ss => ss.Name == stateName);
            return superState != null
                ? _stateFragmentHelper.GetAllSubStates(superState).Except(new [] { stateName }).ToArray()
                : Array.Empty<string>();
        }

        private Transition[] GetOutboundTransitions(StateFragment[] fragments, SuperState[] allSuperStates, string state)
        {
            var superState = _stateFragmentHelper.GetSuperState(allSuperStates, state);
            return superState != null
                ? superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From == state)
                    .Where(t => t.To != state)
                    .ToArray()
                : fragments
                    .OfType<Transition>()
                    .Where(t => t.From == state)
                    .Where(t => t.To != state)
                    .ToArray();
        }

        private Transition[] GetInboundTransitions(Transition[] allTransitions, string state)
        {
            return allTransitions
                .Where(t => t.To == state)
                .Where(t => t.From != t.To)
                .ToArray();
        }

    }
}
