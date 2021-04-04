namespace EtAlii.Generators.Stateless
{
    using System.Linq;

    public abstract partial class StateFragment
    {

        public static Transition[] GetOutboundTransitions(StateMachine stateMachine, string state)
        {
            var superState = GetSuperState(stateMachine.StateFragments, state);
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
        public static Transition[] GetInboundTransitions(StateFragment[] fragments, string state)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.To == state)
                .Where(t => t.From != t.To)
                .ToArray();
        }

        public static Transition[] GetInternalTransitions(StateFragment[] fragments, string state)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.To == state && t.To == t.From)
                .ToArray();
        }

        public static Transition[] GetSyncTransitions(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Where(t => !t.IsAsync)
                .ToArray();
        }

        public static Transition[] GetAsyncTransitions(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.IsAsync)
                .ToArray();
        }

        public static SuperState GetSuperState(StateFragment[] fragments, string substate)
        {
            if (substate == StatelessWriter.BeginStateName || substate == StatelessWriter.EndStateName)
            {
                return null;
            }

            return GetAllSuperStates(fragments)
                .SingleOrDefault(ss =>
                {
                    var isDefined = ss.StateFragments.OfType<StateDescription>().Any(sd => sd.State == substate);
                    var isSuperState = ss.StateFragments.OfType<SuperState>().Any(sd => sd.Name == substate);
                    var isOutbound = ss.StateFragments.OfType<Transition>().Any(sd => sd.From == substate);
                    return isDefined || isSuperState || isOutbound;
                });
        }

        public static string[] GetAllTriggers(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Select(t => t.Trigger)
                .OrderBy(t => t)
                .Distinct() // That is, of course without any doubles.
                .ToArray();
        }

        public static Transition[] GetAllTransitions(StateFragment[] fragments)
        {
            var stateMachineTransitions = fragments
                .OfType<Transition>()
                .ToArray();
            var superStateTransitions = fragments
                .OfType<SuperState>()
                .SelectMany(ss => GetAllTransitions(ss.StateFragments));
            var allTransitions = stateMachineTransitions
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
        public static Transition[] GetUniqueParameterTransitions(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Select(t => new { Transition = t, ParametersAsKey = $"{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.First().Transition)
                .ToArray();
        }

        public static string[] GetAllStates(StateFragment[] fragments)
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

            var allStates = transitionStates
                .Concat(descriptionStates)
                .Concat(superStates)
                .Where(s => s != null)
                .OrderBy(s => s)
                .Distinct() // That is, of course without any doubles.
                .ToArray();
            return allStates;
        }

        public static SuperState[] GetAllSuperStates(StateFragment[] fragments)
        {
            var superStates = fragments
                .OfType<SuperState>()
                .SelectMany(ss => GetAllSuperStates(ss.StateFragments).Concat(new[] {ss}))
                .ToArray();
            return superStates;
        }
    }
}
