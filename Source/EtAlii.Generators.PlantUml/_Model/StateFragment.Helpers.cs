namespace EtAlii.Generators.PlantUml
{
    using System.Collections.Generic;
    using System.Linq;
    using Serilog;

    public class StateFragmentHelper
    {
        private readonly IStateMachineLifetime _lifetime;
        private readonly ILogger _log = Log.ForContext<StateFragmentHelper>();

        public StateFragmentHelper(IStateMachineLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public Transition[] GetOutboundTransitions(StateMachine stateMachine, string state)
        {
            var superState = GetSuperState(stateMachine, state);
            return superState != null
                ? superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From == state)
                    .Where(t => t.To != state)
                    .ToArray()
                : stateMachine.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From == state)
                    .Where(t => t.To != state)
                    .ToArray();
        }

        public bool HasOnlyAsyncInboundTransitions(StateMachine stateMachine, string state)
        {
            var inboundTransitions = GetInboundTransitions(stateMachine.StateFragments, state);

            var superState = GetAllSuperStates(stateMachine.StateFragments)
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = GetAllSubStates(superState);
                var allTransitions = GetAllTransitions(stateMachine.StateFragments);
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

        public bool HasOnlyAsyncOutboundTransitions(StateMachine stateMachine, string state)
        {
            var outboundTransitions = GetOutboundTransitions(stateMachine, state);

            var superState = GetAllSuperStates(stateMachine.StateFragments)
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = GetAllSubStates(superState);
                var allTransitions = GetAllTransitions(stateMachine.StateFragments);
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

        public Transition[] GetInboundTransitions(StateFragment[] fragments, string state)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.To == state)
                .Where(t => t.From != t.To)
                .ToArray();
        }

        public Transition[] GetInternalTransitions(StateFragment[] fragments, string state)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.To == state && t.To == t.From)
                .ToArray();
        }

        public Transition[] GetSyncTransitions(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Where(t => !t.IsAsync)
                .ToArray();
        }

        public Transition[] GetAsyncTransitions(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.IsAsync)
                .ToArray();
        }

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

        public SuperState GetSuperState(StateMachine stateMachine, string substate)
        {
            _log.Debug("Finding super state for: {SubState}", substate);
            if (substate == _lifetime.BeginStateName || substate == _lifetime.EndStateName)
            {
                return null;
            }

            var superState = GetAllSuperStates(stateMachine.StateFragments)
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

        public string[] GetAllTriggers(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Select(t => t.Trigger)
                .OrderBy(t => t)
                .Distinct() // That is, of course without any doubles.
                .ToArray();
        }

        public Transition[] GetAllTransitions(StateFragment[] fragments)
        {
            var transitions = fragments
                .OfType<Transition>()
                .ToArray();
            var superStateTransitions = fragments
                .OfType<SuperState>()
                .Select(ss => ss.StateFragments)
                .SelectMany(GetAllTransitions);
            var allTransitions = transitions
                .Concat(superStateTransitions)
                .ToArray();
            return allTransitions;
        }

        /// <summary>
        /// We want to know all unique transitions defined in the diagram.
        /// That is, the transitions grouped by the trigger and unique sequence of parameters.
        /// </summary>
        /// <param name="fragments"></param>
        /// <returns></returns>
        public Transition[] GetUniqueParameterTransitions(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Select(t => new { Transition = t, ParametersAsKey = $"{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.First().Transition)
                .ToArray();
        }

        public string[] GetAllSubStates(SuperState superState)
        {
            var transitionStates = superState.StateFragments
                .OfType<Transition>()
                .SelectMany(t => new[] { t.From })
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

        public string[] GetAllStates(StateFragment[] fragments)
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
