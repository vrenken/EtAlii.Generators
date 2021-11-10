namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class MethodCall
    {
        public State State { get; }
        public bool IsSuperState { get; }

        public MethodCall(State state, bool isSuperState)
        {
            State = state;
            IsSuperState = isSuperState;
        }
    }
}
