namespace EtAlii.Generators.PlantUml
{
    public record State
    {
        public string Name { get; init; }
        public State[] Children { get; init; }
        public State[] AllChildren { get; init; }

        public string[] AllSubStates { get; init; }
        public Transition[] AllTransitions { get; init; }

        public SuperState Parent { get; init; }
        public SuperState[] AllParents { get; init; }

        public Transition[] InboundTransitions { get; init; }
        public Transition[] OutboundTransitions { get; init; }
        public Transition[] InternalTransitions { get; init; }

        public bool HasOnlyAsyncInboundTransitions { get; init; }
        public bool HasOnlyAsyncOutboundTransitions { get; init; }

    }
}
