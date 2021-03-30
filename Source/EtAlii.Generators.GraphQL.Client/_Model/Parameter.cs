namespace EtAlii.Generators.GraphQL.Client
{
    public class Parameter
    {
        public string Type { get; }

        public string Name { get; }

        public bool HasName => !string.IsNullOrEmpty(Name);

        public SourcePosition Source { get; }

        public Parameter(string type, string name, SourcePosition source)
        {
            Type = type;
            Name = name;
            Source = source;
        }
    }
}
