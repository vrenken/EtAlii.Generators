namespace EtAlii.Generators.PlantUml
{
    using System.Linq;

    public partial class StateFragmentHelper
    {
        public string[] GetAllTriggers(StateFragment[] fragments)
        {
            return GetAllTransitions(fragments)
                .Select(t => t.Trigger)
                .OrderBy(t => t)
                .Distinct() // That is, of course without any doubles.
                .ToArray();
        }
    }
}
