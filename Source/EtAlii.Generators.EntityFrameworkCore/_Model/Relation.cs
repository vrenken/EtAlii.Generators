namespace EtAlii.Generators.EntityFrameworkCore
{
    public class Relation
    {
        public string From { get; }
        public string To { get; }

        public SourcePosition Source { get; }

        public Relation(string from, string to, SourcePosition source)
        {
            From = from;
            To = to;
            Source = source;
        }
    }
}
