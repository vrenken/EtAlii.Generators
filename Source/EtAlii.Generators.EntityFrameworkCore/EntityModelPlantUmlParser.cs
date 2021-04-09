namespace EtAlii.Generators.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// The central class responsible of parsing both the PlantUML and EntityFramework specific constructions
    /// from the given input file.
    /// </summary>
    public class EntityModelPlantUmlParser : IParser<EntityModel>
    {
        public bool TryParse(AdditionalText file, List<string> log, out EntityModel model, out Diagnostic[] diagnostics)
        {
            var success = false;
            var diagnosticErrors = new List<Diagnostic>();
            try
            {
                var plantUmlText = file.GetText()?.ToString();

                log.Add("========================");
                log.Add($"Parsing PlantUml file: {file.Path}");
                log.Add(plantUmlText);

                var inputStream = new AntlrInputStream(plantUmlText);
                var lexer = new PlantUmlLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new PlantUmlParser(commonTokenStream);
                var errorListener = new ParsingErrorListener(file.Path, GeneratorRule.InvalidPlantUmlStateMachine);
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);
                var parsingContext = parser.model();

                var originalFileName = Path.GetFileName(file.Path);
                var visitor = new PlantUmlVisitor(originalFileName);

                model = visitor.VisitModel(parsingContext) as EntityModel;

                if (parser.NumberOfSyntaxErrors != 0)
                {
                    log.Add($"NumberOfSyntaxErrors: {parser.NumberOfSyntaxErrors}");

                    diagnosticErrors.AddRange(errorListener.Diagnostics);
                }

                if (parsingContext.exception != null)
                {
                    log.Add($"Parser exception: {parsingContext.exception.Message}");
                    var location = Location.Create(file.Path, TextSpan.FromBounds(0,0), new LinePositionSpan(LinePosition.Zero, LinePosition.Zero));
                    var diagnostic = Diagnostic.Create(GeneratorRule.ParsingThrewException, location, parsingContext.exception.Message, parsingContext.exception.StackTrace);
                    diagnosticErrors.Add(diagnostic);
                }

                success = !diagnosticErrors.Any();
                if (success)
                {
                    log.Add("Parsed PlantUml");
                }
                else
                {
                    log.Add("Failed parsed PlantUml");

                    model = null;
                }
            }
            catch (Exception e)
            {
                log.Add($"Unable to parse PlantUml: {e.Message}");
                log.Add($"{e.StackTrace}");

                var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(GeneratorRule.ParsingThrewException, location, e.Message, e.StackTrace);
                diagnosticErrors.Add(diagnostic);

                model = null;
            }

            diagnostics = diagnosticErrors.ToArray();
            return success;
        }
    }
}
