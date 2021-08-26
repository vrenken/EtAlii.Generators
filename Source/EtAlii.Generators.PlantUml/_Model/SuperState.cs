namespace EtAlii.Generators.PlantUml
{
    public class SuperState : StateFragment
    {
        public StereoType StereoType { get; }
        public string Name { get; }
        public StateFragment[] StateFragments { get; }

        public SourcePosition Source { get; }

        public SuperState(
            string name,
            StateFragment[] stateFragments,
            SourcePosition source,
            StereoType stereoType)
        {
            Name = name;
            StateFragments = stateFragments;
            Source = source;
            StereoType = stereoType;
        }
    }
}
