namespace EtAlii.Generators.GraphQL.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Serilog;

    /// <summary>
    /// The central class responsible of parsing both the PlantUML and Stateless specific constructions
    /// from the given input file.
    /// </summary>
    public class GraphQLQueryParser : IParser<object>
    {
        private readonly ILogger _log = Log.ForContext<GraphQLQueryParser>();

        public bool TryParse(AdditionalText file, out object stateMachine, out Diagnostic[] diagnostics)
        {
            var success = false;
            var diagnosticErrors = new List<Diagnostic>();
            try
            {
                var plantUmlText = file.GetText()?.ToString();

                _log
                    .ForContext("FileContent", plantUmlText)
                    .Information("Parsing GraphQL query {File}", file.Path);

                var inputStream = new AntlrInputStream(plantUmlText);
                var lexer = new GraphQLLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new GraphQLParser(commonTokenStream);
                var errorListener = new ParsingErrorListener(file.Path, DiagnosticRule.InvalidPlantUmlStateMachine);
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);
                var parsingContext = parser.document();

                var visitor = new GraphQLVisitor();
                stateMachine = visitor.VisitDocument(parsingContext) as StateMachine;

                if (parser.NumberOfSyntaxErrors != 0)
                {
                    _log.Information("File parsed with {NumberOfSyntaxErrors}", parser.NumberOfSyntaxErrors);

                    diagnosticErrors.AddRange(errorListener.Diagnostics);
                }

                if (parsingContext.exception != null)
                {
                    _log.Fatal(parsingContext.exception, "Parsing threw an exception");
                    var location = Location.Create(file.Path, TextSpan.FromBounds(0,0), new LinePositionSpan(LinePosition.Zero, LinePosition.Zero));
                    var diagnostic = Diagnostic.Create(DiagnosticRule.ParsingThrewException, location, parsingContext.exception.Message, parsingContext.exception.StackTrace);
                    diagnosticErrors.Add(diagnostic);
                }

                success = !diagnosticErrors.Any();
                if (success)
                {
                    _log.Information("Successfully parsed GraphQL query file");
                }
                else
                {
                    _log.Error("Unable to parse GraphQL query file");
                    stateMachine = null;
                }
            }
            catch (Exception e)
            {
                _log.Fatal(e, "Failure parsing GraphQL query file");

                var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(DiagnosticRule.ParsingThrewException, location, e.Message, e.StackTrace);
                diagnosticErrors.Add(diagnostic);

                stateMachine = null;
            }

            diagnostics = diagnosticErrors.ToArray();
            return success;
        }
    }
}
