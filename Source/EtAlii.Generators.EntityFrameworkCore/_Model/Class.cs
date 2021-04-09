namespace EtAlii.Generators.EntityFrameworkCore
{
    public class Class
    {
        public string Name { get; }

        public SourcePosition Source { get; }

        public Class(string name, SourcePosition source)
        {
            Name = name;
            Source = source;
        }
    }
}
