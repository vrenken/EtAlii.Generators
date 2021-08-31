namespace EtAlii.Generators.MicroMachine
{
    public class MethodCall
    {
        public string State { get; }
        public bool IsAsync { get; }
        public bool IsSuperState { get; }

        public MethodCall(string state, bool isSuperState, bool isAsync)
        {
            State = state;
            IsSuperState = isSuperState;
            IsAsync = isAsync;
        }
    }
}
