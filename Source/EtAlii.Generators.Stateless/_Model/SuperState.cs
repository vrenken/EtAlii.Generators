namespace EtAlii.Generators.Stateless
{
    public class SuperState : StateFragment
    {
        public string Name { get; }
        public StateFragment[] StateFragments { get; }

        public SuperState(string name, StateFragment[] stateFragments)
        {
            Name = name;
            StateFragments = stateFragments;
        }
    }
}
