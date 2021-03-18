namespace EtAlii.Generators.Stateless
{
    public class StateTransition : StateFragment
    {
        private readonly TransitionDetails _details;
        public string From { get; }
        public string To { get; }
        public string Trigger =>_details.Name;

        public bool IsAsync => _details.IsAsync;

        public Parameter[] Parameters => _details.Parameters;

        public StateTransition(string from, string to, TransitionDetails details)
        {
            _details = details;
            From = from;
            To = to;
        }
    }
}
