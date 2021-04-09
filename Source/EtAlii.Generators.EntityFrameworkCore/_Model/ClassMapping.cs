
namespace EtAlii.Generators.EntityFrameworkCore
{
    public class ClassMapping
    {
        public string Name { get; }

        public SourcePosition Source { get; }

        public ClassMapping(string name, SourcePosition source)
        {
            Name = name;
            Source = source;
        }
    }
}
