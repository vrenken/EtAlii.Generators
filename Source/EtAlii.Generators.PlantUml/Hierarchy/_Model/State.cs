namespace EtAlii.Generators.PlantUml
{
    public record State
    {
        public string Name { get; init; }
        public State[] Children { get; init; }
        public Transition[] Transitions { get; init; }

        public bool HasOnlyAsyncInboundTransitions { get; init; }
        public bool HasOnlyAsyncOutboundTransitions { get; init; }
    }
}
