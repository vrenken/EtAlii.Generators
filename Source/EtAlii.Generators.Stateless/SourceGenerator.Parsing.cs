namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public partial class SourceGenerator
    {
        private static readonly DiagnosticDescriptor _invalidPlantUmlStateMachineRule = new
        (
            id: "SL1001",
            title: "PlantUml file is invalid",
            messageFormat: "PlantUml file is invalid: {0}",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        private static readonly DiagnosticDescriptor _plantUmlStateMachineParsingThrowsExceptionRule = new
        (
            id: "SL1002",
            title: "PlantUml parsing throws exception",
            messageFormat: "PlantUml parsing throws exception: '{0}' {1}",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        private static readonly DiagnosticDescriptor _noStartStatesDefinedRule = new
        (
            id: "SL1003",
            title: "No start states defined",
            messageFormat: "No start states defined",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        private static readonly DiagnosticDescriptor _unnamedParameterRule = new
        (
            id: "SL1004",
            title: "Trigger definition uses unnamed parameters - Consider using named parameters",
            messageFormat: "Trigger definition uses unnamed parameters: '{0}' - Consider using named parameters",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        private static readonly DiagnosticDescriptor _plantUmlStateMachineProcessingThrowsExceptionRule = new
        (
            id: "SL1005",
            title: "PlantUml processing throws exception",
            messageFormat: "PlantUml processing throws exception: '{0}' {1}",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        private bool TryParsePlantUml(AdditionalText file, List<string> log, out StateMachine stateMachine, out Diagnostic[] diagnostics)
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
                var errorListener = new PlantUmlErrorListener(file.Path, _invalidPlantUmlStateMachineRule);
                parser.RemoveErrorListeners();
                parser.AddErrorListener(errorListener);
                var parsingContext = parser.state_machine();

                var visitor = new PlantUmlVisitor();
                stateMachine = visitor.VisitState_machine(parsingContext) as StateMachine;

                if (parser.NumberOfSyntaxErrors != 0)
                {
                    log.Add($"NumberOfSyntaxErrors: {parser.NumberOfSyntaxErrors}");

                    diagnosticErrors.AddRange(errorListener.Diagnostics);
                }

                if (parsingContext.exception != null)
                {
                    log.Add($"Parser exception: {parsingContext.exception.Message}");
                    var location = Location.Create(file.Path, TextSpan.FromBounds(0,0), new LinePositionSpan(LinePosition.Zero, LinePosition.Zero));
                    var diagnostic = Diagnostic.Create(_plantUmlStateMachineParsingThrowsExceptionRule, location, parsingContext.exception.Message, parsingContext.exception.StackTrace);
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

                    stateMachine = null;
                }
            }
            catch (Exception e)
            {
                log.Add($"Unable to parse PlantUml: {e.Message}");
                log.Add($"{e.StackTrace}");

                var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(_plantUmlStateMachineParsingThrowsExceptionRule, location, e.Message, e.StackTrace);
                diagnosticErrors.Add(diagnostic);

                stateMachine = null;
            }

            diagnostics = diagnosticErrors.ToArray();
            return success;
        }
    }
}
