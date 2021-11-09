namespace EtAlii.Generators.PlantUml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class StateFragmentHelper
    {
        public SuperState[] GetAllSuperStates(StateMachine stateMachine, string substate)
        {
            _log.Debug("Finding all super states for: {SubState}", substate);

            var result = new List<SuperState>();

            var superState = GetSuperState(stateMachine, substate);
            if (superState != null)
            {
                do
                {
                    result.Add(superState);
                    substate = superState.Name;
                    superState = GetSuperState(stateMachine, substate);

                } while (superState != null);
            }

            return result.ToArray();
        }

        private readonly Dictionary<StateFragment[], SuperState[]> _allSuperStates = new ();
        public SuperState[] GetAllSuperStates(StateFragment[] fragments)
        {
            if(!_allSuperStates.TryGetValue(fragments, out var superStates))
            {
                _allSuperStates[fragments] = superStates = fragments
                    .OfType<SuperState>()
                    .SelectMany(ss => GetAllSuperStates(ss.StateFragments).Concat(new[] {ss}))
                    .ToArray();

            }
            return superStates;
        }

        public SuperState GetSuperState(StateMachine stateMachine, string substate) => GetSuperState(stateMachine.StateFragments, substate);

        public SuperState GetSuperState(StateFragment[] fragments, string substate)
        {
            _log.Debug("Finding super state for: {SubState}", substate);
            if (substate == _lifetime.BeginStateName || substate == _lifetime.EndStateName)
            {
                return null;
            }

            var superState = GetAllSuperStates(fragments)
                .SingleOrDefault(ss =>
                {
                    var isDefined = ss.StateFragments.OfType<StateDescription>().Any(sd => sd.State == substate);
                    var isSuperState = ss.StateFragments.OfType<SuperState>().Any(sd => sd.Name == substate);
                    var isOutbound = ss.StateFragments.OfType<Transition>().Any(sd => sd.From == substate);
                    var isInbound = ss.StateFragments.OfType<Transition>().Any(sd => sd.To == substate);
                    return isDefined || isSuperState || isOutbound || isInbound;
                });
            return superState?.Name != substate ? superState : null;
        }

        public string[] GetAllSubStates(StateMachine stateMachine, string stateName) => GetAllSubStates(stateMachine.StateFragments, stateName);

        public string[] GetAllSubStates(StateFragment[] fragments, string stateName)
        {
            var superState = GetAllSuperStates(fragments)
                .SingleOrDefault(ss => ss.Name == stateName);
            return superState != null
                ? GetAllSubStates(superState).Except(new [] { stateName }).ToArray()
                : Array.Empty<string>();
        }

        public string[] GetAllSubStates(SuperState superState)
        {
            var transitionStates = superState.StateFragments
                .OfType<Transition>()
                .SelectMany(t => new[] { t.From, t.To })
                .ToArray();
            var descriptionStates = superState.StateFragments
                .OfType<StateDescription>()
                .SelectMany(t => new[] { t.State })
                .ToArray();
            var superStates = superState.StateFragments
                .OfType<SuperState>()
                .Select(s => s.Name )
                .ToArray();
            var subStates = superState.StateFragments
                .OfType<SuperState>()
                .SelectMany(GetAllSubStates);

            var allStates = transitionStates
                .Concat(descriptionStates)
                .Concat(superStates)
                .Concat(subStates)
                .Where(s => s != null)
                .OrderBy(s => s)
                .Distinct() // That is, of course without any doubles.
                .ToArray();
            return allStates;
        }

        internal string[] GetAllStates(StateFragment[] fragments)
        {
            var transitionStates = GetAllTransitions(fragments)
                .SelectMany(t => new[] { t.From, t.To })
                .ToArray();
            var descriptionStates = fragments
                .OfType<StateDescription>()
                .SelectMany(t => new[] { t.State })
                .ToArray();
            var superStates = fragments
                .OfType<SuperState>().Select(s => s.Name )
                .ToArray();
            var subStates = fragments
                .OfType<SuperState>()
                .Select(ss => ss.StateFragments)
                .SelectMany(GetAllStates);

            var allStates = transitionStates
                .Concat(descriptionStates)
                .Concat(superStates)
                .Concat(subStates)
                .Where(s => s != null)
                .OrderBy(s => s)
                .Distinct() // That is, of course without any doubles.
                .ToArray();
            return allStates;
        }
    }
}
