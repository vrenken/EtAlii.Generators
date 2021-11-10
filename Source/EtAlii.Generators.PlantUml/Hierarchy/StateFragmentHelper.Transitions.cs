namespace EtAlii.Generators.PlantUml
{
    using System.Linq;

    public partial class StateFragmentHelper
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
