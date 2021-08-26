namespace EtAlii.Generators.PlantUml
{
    public class TransitionDetails
    {
        public string Name { get; set; }
        public bool HasConcreteName { get; }

        public TransitionDetails(string name, bool hasConcreteName)
        {
            Name = name;
            HasConcreteName = hasConcreteName;
        }
    }
}
