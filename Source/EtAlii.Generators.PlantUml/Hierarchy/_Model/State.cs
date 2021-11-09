namespace EtAlii.Generators.PlantUml
{
    public record State
    {
        public string Name { get; init; }
        public State[] Children { get; init; }
        public Transition[] AllTransitions { get; init; }

        public Transition[] InboundTransitions { get; init; }
        public Transition[] OutboundTransitions { get; init; }
        public Transition[] InternalTransitions { get; init; }
        public bool HasOnlyAsyncInboundTransitions { get; init; }
        public bool HasOnlyAsyncOutboundTransitions { get; init; }

    }
}
