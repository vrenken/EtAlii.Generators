namespace EtAlii.Generators.GraphQL.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// The central class responsible of parsing both the PlantUML and Stateless specific constructions
    /// from the given input file.
    /// </summary>
    public class GraphQLQueryParser : IParser<object>
    {
        public bool TryParse(AdditionalText file, List<string> log, out object stateMachine, out Diagnostic[] diagnostics)
        {
            var success = false;
            var diagnosticErrors = new List<Diagnostic>();
            try
            {
                var plantUmlText = file.GetText()?.ToString();

                log.Add("========================");
                log.Add($"Parsing GraphQL query file: {file.Path}");
                log.Add(plantUmlText);

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
                    log.Add($"NumberOfSyntaxErrors: {parser.NumberOfSyntaxErrors}");

                    diagnosticErrors.AddRange(errorListener.Diagnostics);
                }

                if (parsingContext.exception != null)
                {
                    log.Add($"Parser exception: {parsingContext.exception.Message}");
                    var location = Location.Create(file.Path, TextSpan.FromBounds(0,0), new LinePositionSpan(LinePosition.Zero, LinePosition.Zero));
                    var diagnostic = Diagnostic.Create(DiagnosticRule.ParsingThrewException, location, parsingContext.exception.Message, parsingContext.exception.StackTrace);
                    diagnosticErrors.Add(diagnostic);
                }

                success = !diagnosticErrors.Any();
                if (success)
                {
                    log.Add("Parsed GraphQL query");
                }
                else
                {
                    log.Add("Failed parsing GraphQL query");

                    stateMachine = null;
                }
            }
            catch (Exception e)
            {
                log.Add($"Unable to parse GraphQL query: {e.Message}");
                log.Add($"{e.StackTrace}");

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
