namespace EtAlii.Generators.EntityFrameworkCore
{
    public class Class
    {
        public string Name { get; }

        public Property[] Properties { get; }

        public SourcePosition Source { get; }

        public ClassMapping Mapping { get; }

        public Class(string name, Property[] properties, ClassMapping classMapping, SourcePosition source)
        {
            Name = name;
            Source = source;
            Properties = properties;
            Mapping = classMapping;
        }
    }
}
