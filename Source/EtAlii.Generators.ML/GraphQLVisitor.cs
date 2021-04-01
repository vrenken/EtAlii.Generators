namespace EtAlii.Generators.ML
{
    /// <summary>
    /// An implementation of the visitor generated using the Antlr4 g4 parser and lexer.
    /// Antlr4 is very fascinating technology if you'd ask me...
    /// </summary>
    public class GraphQLVisitor : GraphQLBaseVisitor<object>
    {
        public override object VisitDocument(GraphQLParser.DocumentContext context)
        {
            // Action: Return the right object hierarchy.
            return base.VisitDocument(context);
        }
    }
}
