namespace EtAlii.Generators.GraphQL.Client
{
    /// <summary>
    /// An implementation of the visitor generated using the Antlr4 g4 parser and lexer.
    /// Antlr4 is very fascinating technology if you'd ask me...
    /// </summary>
    public class GraphQLVisitor : GraphQLBaseVisitor<object>
    {
        public override object VisitDocument(GraphQLParser.DocumentContext context)
        {
            return base.VisitDocument(context);
        }
    }
}
