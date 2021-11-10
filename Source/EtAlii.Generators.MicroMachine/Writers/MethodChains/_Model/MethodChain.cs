namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class MethodChain
    {
        public State From { get; init; }
        public State To { get; init; }
        public MethodCall[] ExitCalls { get; init; }
        public MethodCall[] EntryCalls { get; init; }
    }
}
