namespace EtAlii.Generators.PlantUml
{
    using System.Linq;

    public partial class StateFragmentHelper
    {
        public Transition[] GetOutboundTransitions(StateMachine stateMachine, string state) => GetOutboundTransitions(stateMachine.StateFragments, state);

        private Transition[] GetOutboundTransitions(StateFragment[] fragments, string state)
        {
            var superState = GetSuperState(fragments, state);
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

        public bool HasOnlyAsyncInboundTransitions(StateMachine stateMachine, string state) => HasOnlyAsyncInboundTransitions(stateMachine.StateFragments, state);

        public bool HasOnlyAsyncInboundTransitions(StateFragment[] fragments, string state)
        {
            var inboundTransitions = GetInboundTransitions(fragments, state);

            var superState = GetAllSuperStates(fragments)
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = GetAllSubStates(superState);
                var allTransitions = GetAllTransitions(fragments);
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

        public bool HasOnlyAsyncOutboundTransitions(StateMachine stateMachine, string state) => HasOnlyAsyncOutboundTransitions(stateMachine.StateFragments, state);

        public bool HasOnlyAsyncOutboundTransitions(StateFragment[] fragments, string state)
        {
            var outboundTransitions = GetOutboundTransitions(fragments, state);

            var superState = GetAllSuperStates(fragments)
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = GetAllSubStates(superState);
                var allTransitions = GetAllTransitions(fragments);
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
    }
}
