namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class MethodCall
    {
        public State State { get; }
        public bool IsSuperState { get; }

        public bool IsAsync { get; }

        public MethodCall(State state, bool isSuperState, bool isAsync)
        {
            State = state;
            IsSuperState = isSuperState;
            IsAsync = isAsync;
        }
    }
}
