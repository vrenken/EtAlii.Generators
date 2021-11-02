namespace EtAlii.Generators.PlantUml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Serilog;

    /// <summary>
    /// The central class responsible of parsing PlantUML specific state machine constructions
    /// from the given input file.
    /// </summary>
    public class PlantUmlStateMachineParser : IParser<StateMachine>
    {
        private readonly IStateMachineLifetime _lifetime;
        private readonly ILogger _log = Log.ForContext<PlantUmlStateMachineParser>();

        public PlantUmlStateMachineParser(IStateMachineLifetime lifetime)
        {
            _lifetime = lifetime;
        }

        public bool TryParse(AdditionalText file, out StateMachine stateMachine, out Diagnostic[] diagnostics)
        {
            var success = false;
            var diagnosticErrors = new List<Diagnostic>();
            try
            {
                var plantUmlText = file.GetText()?.ToString();

                _log
                    .ForContext("FileContent", plantUmlText)
                    .Information("Parsing PlantUml {File}", file.Path);

                var inputStream = new AntlrInputStream(plantUmlText);
                var lexer = new PlantUmlLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new PlantUmlParser(commonTokenStream);
                var errorListener = new ParsingErrorListener(file.Path, GeneratorRule.InvalidPlantUmlStateMachine);
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);
                var parsingContext = parser.state_machine();

                var originalFileName = Path.GetFileName(file.Path);
                var visitor = new PlantUmlVisitor(originalFileName, _lifetime);

                stateMachine = visitor.VisitState_machine(parsingContext) as StateMachine;

                if (parser.NumberOfSyntaxErrors != 0)
                {
                    _log.Information("File parsed with {NumberOfSyntaxErrors}", parser.NumberOfSyntaxErrors);

                    diagnosticErrors.AddRange(errorListener.Diagnostics);
                }

                if (parsingContext.exception != null)
                {
                    _log.Fatal(parsingContext.exception, "Parsing threw an exception");
                    var location = Location.Create(file.Path, TextSpan.FromBounds(0,0), new LinePositionSpan(LinePosition.Zero, LinePosition.Zero));
                    var diagnostic = Diagnostic.Create(GeneratorRule.ParsingThrewException, location, parsingContext.exception.Message, parsingContext.exception.StackTrace);
                    diagnosticErrors.Add(diagnostic);
                }

                success = !diagnosticErrors.Any();
                if (success)
                {
                    _log.Information("Successfully parsed PlantUml file");
                }
                else
                {
                    _log.Error("Unable to parse PlantUml file");
                    stateMachine = null;
                }
            }
            catch (Exception e)
            {
                _log.Fatal(e, "Failure parsing PlantUml file");

                var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(GeneratorRule.ParsingThrewException, location, e.Message, e.StackTrace);
                diagnosticErrors.Add(diagnostic);

                stateMachine = null;
            }

            diagnostics = diagnosticErrors.ToArray();
            return success;
        }
    }
}
