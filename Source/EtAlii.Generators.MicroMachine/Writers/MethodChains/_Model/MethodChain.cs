namespace EtAlii.Generators.MicroMachine
{
    public class MethodChain
    {
        public string From { get; init; }
        public string To { get; init; }
        public MethodCall[] ExitCalls { get; init; }
        public MethodCall[] EntryCalls { get; init; }
    }
}
