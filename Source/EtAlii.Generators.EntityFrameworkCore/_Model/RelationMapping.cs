
namespace EtAlii.Generators.EntityFrameworkCore
{
    public class RelationMapping
    {
        public string FromProperty { get; }

        public string ToProperty { get; }

        public SourcePosition Source { get; }

        public RelationMapping(string fromProperty, string toProperty, SourcePosition source)
        {
            FromProperty = fromProperty;
            ToProperty = toProperty;
            Source = source;
        }
    }
}
