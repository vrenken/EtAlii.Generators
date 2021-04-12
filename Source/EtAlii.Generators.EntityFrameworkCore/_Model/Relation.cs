namespace EtAlii.Generators.EntityFrameworkCore
{
    public class Relation
    {
        public string From { get; }

        public Cardinality FromCardinality { get; }
        public string To { get; }

        public Cardinality ToCardinality { get; }
        public RelationMapping Mapping { get; }

        public SourcePosition Source { get; }

        public Relation(string from, Cardinality fromCardinality, string to, Cardinality toCardinality, RelationMapping relationMapping, SourcePosition source)
        {
            From = from;
            FromCardinality = fromCardinality;
            To = to;
            ToCardinality = toCardinality;
            Source = source;
            Mapping = relationMapping;
        }
    }
}
