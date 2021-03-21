namespace EtAlii.Generators.Stateless
{
    public class StateTransition : StateFragment
    {
        private readonly TransitionDetails _details;

        public string From { get; }

        public string To { get; }

        public string Trigger =>_details.Name;

        public bool HasConcreteTriggerName =>_details.HasConcreteName;

        public bool IsAsync => _details.IsAsync;

        public Parameter[] Parameters => _details.Parameters;

        public SourcePosition Source { get; }

        public StateTransition(string from, string to, TransitionDetails details, SourcePosition source)
        {
            _details = details;
            Source = source;
            From = from;
            To = to;
        }
    }
}
