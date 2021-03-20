namespace EtAlii.Generators.Stateless
{
    public class Parameter
    {
        public string Type { get; }
        public string Name { get; }

        public bool HasName => !string.IsNullOrEmpty(Name);

        public int SourceLine { get; }
        public int SourceColumn { get; }

        public Parameter(string type, string name, int sourceLine, int sourceColumn)
        {
            Type = type;
            Name = name;
            SourceLine = sourceLine;
            SourceColumn = sourceColumn;
        }
    }
}
