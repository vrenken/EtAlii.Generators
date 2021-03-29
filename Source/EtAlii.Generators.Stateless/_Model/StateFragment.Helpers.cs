namespace EtAlii.Generators.Stateless
{
    using System.Linq;

    public abstract partial class StateFragment
    {

        public static Transition[] GetOutboundTransitions(StateFragment[] fragments, string state)
        {
            return GetAllTransitions(fragments)
                .Where(t => t.From == state)
                .Where(t => t.From != t.To)
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

        public static string[] GetAllStates(StateFragment[] fragments)
        {
            var allTransitions = GetAllTransitions(fragments);

            var transitionStates = allTransitions.SelectMany(t => new[] { t.From, t.To });
            var descriptionStates = fragments.OfType<StateDescription>().SelectMany(t => new[] { t.State });
            var superStates = fragments.OfType<SuperState>().Select(s => s.Name );
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
