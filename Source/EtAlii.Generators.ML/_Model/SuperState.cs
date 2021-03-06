namespace EtAlii.Generators.ML
{
    public class SuperState : StateFragment
    {
        public string Name { get; }
        public StateFragment[] StateFragments { get; }

        public SourcePosition Source { get; }

        public SuperState(
            string name,
            StateFragment[] stateFragments,
            SourcePosition source)
        {
            Name = name;
            StateFragments = stateFragments;
            Source = source;
        }
    }
}
