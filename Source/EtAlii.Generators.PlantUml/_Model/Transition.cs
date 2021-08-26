namespace EtAlii.Generators.PlantUml
{
    public class Transition : StateFragment
    {
        private readonly TransitionDetails _transitionDetails;
        private readonly TriggerDetails _triggerDetails;

        public string From { get; }

        public string To { get; }

        public string Trigger =>_transitionDetails.Name;

        public bool HasConcreteTriggerName =>_transitionDetails.HasConcreteName;

        public bool IsAsync => _triggerDetails.IsAsync;

        public Parameter[] Parameters => _triggerDetails.Parameters;

        public SourcePosition Source { get; }

        public Transition(
            string from, string to,
            TransitionDetails transitionDetails,
            TriggerDetails triggerDetails,
            SourcePosition source)
        {
            _transitionDetails = transitionDetails;
            _triggerDetails = triggerDetails;
            Source = source;
            From = from;
            To = to;
        }
    }
}
