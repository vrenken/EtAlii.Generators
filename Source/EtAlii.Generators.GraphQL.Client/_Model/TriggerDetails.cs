namespace EtAlii.Generators.GraphQL.Client
{
    public class TriggerDetails
    {
        public bool IsAsync { get; }
        public Parameter[] Parameters { get; }

        public TriggerDetails(bool isAsync, Parameter[] parameters)
        {
            IsAsync = isAsync;
            Parameters = parameters;
        }
    }
}
