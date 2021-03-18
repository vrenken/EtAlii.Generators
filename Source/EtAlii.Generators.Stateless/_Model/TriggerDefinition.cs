namespace EtAlii.Generators.Stateless
{
    public class TriggerDefinition
    {
        public bool IsAsync { get; }
        public Parameter[] Parameters { get; }

        public TriggerDefinition(bool isAsync, Parameter[] parameters)
        {
            IsAsync = isAsync;
            Parameters = parameters;
        }
    }
}
