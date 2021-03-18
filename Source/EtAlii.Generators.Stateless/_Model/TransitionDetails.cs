namespace EtAlii.Generators.Stateless
{
    public class TransitionDetails
    {
        public string Name { get; }

        public bool IsAsync { get; }
        public Parameter[] Parameters { get; }

        public TransitionDetails(string name, bool isAsync, Parameter[] parameters)
        {
            Name = name;
            IsAsync = isAsync;
            Parameters = parameters;
        }
    }
}
