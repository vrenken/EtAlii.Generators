namespace EtAlii.Generators.MicroMachine
{
    public class MethodCall
    {
        public string State { get; }
        public bool IsSuperState { get; }

        public MethodCall(string state, bool isSuperState)
        {
            State = state;
            IsSuperState = isSuperState;
        }
    }
}
