namespace EtAlii.Generators.EntityFrameworkCore
{
    public class Property
    {
        public string Name { get; }
        public string Type { get; }

        public SourcePosition Source { get; }

        public Property(string name, string type, SourcePosition source)
        {
            Name = name;
            Source = source;
            Type = type;
        }
    }
}
