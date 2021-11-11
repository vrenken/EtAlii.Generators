namespace EtAlii.Generators.PlantUml
{
    using System.Linq;

    public class StateFragmentHelper
    {
        internal Transition[] GetAllTransitions(StateFragment[] fragments)
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
        /// <returns></returns>
        public Transition[] GetUniqueParameterTransitions(StateMachine stateMachine)
        {
            return stateMachine.AllTransitions
                .Select(t => new { Transition = t, ParametersAsKey = $"{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.First().Transition)
                .ToArray();
        }
    }
}
