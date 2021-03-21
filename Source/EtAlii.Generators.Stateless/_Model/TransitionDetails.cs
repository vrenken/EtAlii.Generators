namespace EtAlii.Generators.Stateless
{
    public class TransitionDetails
    {
        public string Name { get; set; }
        public bool HasConcreteName { get; }
        public bool IsAsync { get; }
        public Parameter[] Parameters { get; }

        public TransitionDetails(string name, bool isAsync, Parameter[] parameters, bool hasConcreteName)
        {
            Name = name;
            HasConcreteName = hasConcreteName;
            IsAsync = isAsync;
            Parameters = parameters;
        }
    }
}
