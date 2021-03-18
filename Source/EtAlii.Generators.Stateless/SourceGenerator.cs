namespace EtAlii.Generators.Stateless
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// A code generator able to create Stateless source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    [Generator]
    public partial class SourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // For testing and troubleshooting we'll use a simple list of strings that will be dumped to a file
            // at the end of the process.
            var log = new List<string>();

            // For actual troubleshooting of the source diagrams we use the Roslyn specific Diagnostics pattern.
            var diagnostics = new List<Diagnostic>();

            foreach (var file in context.AdditionalFiles)
            {
                // This code generator is only able to understand and parse PlantUml files.
                // Because of that we ignore everything except files with the .puml extension.
                if (Path.GetExtension(file.Path).Equals(".puml", StringComparison.OrdinalIgnoreCase))
                {
                    // First thing to do is parse the .puml file and build an in-memory model.
                    if (TryParsePlantUml(file, log, out var stateMachine, out var parseDiagnostics))
                    {
                        try
                        {
                            var originalFileName = Path.GetFileName(file.Path);

                            // If there is no classname defined in the diagram we'll need to come up with one ourselves.
                            if (!stateMachine.Settings.OfType<ClassStatelessSetting>().Any())
                            {
                                // Let's use a C# safe subset of the characters in the filename.
                                var classNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(originalFileName), "[^a-zA-Z0-9_]", "");
                                stateMachine.Settings.Add(new ClassStatelessSetting(classNameFromFileName));
                            }

                            var fileName = Path.ChangeExtension(originalFileName, "Generated.cs");

                            using var stringWriter = new StringWriter();
                            using var writer = new IndentedTextWriter(stringWriter);
                            var writeContext = CreateWriteContext(writer, originalFileName, log, diagnostics, stateMachine);

                            ValidateStateMachine(writeContext);

                            WriteNamespace(writeContext);

                            var content = stringWriter.ToString();
                            context.AddSource(fileName, content);
                        }
                        catch (Exception e)
                        {
                            log.Add($"File writing throws exception: {e.Message}");
                            log.Add($"{e.StackTrace}");

                            var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                            var diagnostic = Diagnostic.Create(PlantUmlStateMachineProcessingThrowsExceptionRule, location, e.Message, e.StackTrace);
                            diagnostics.Add(diagnostic);
                        }
                    }

                    diagnostics.AddRange(parseDiagnostics);

                    // Report any error/info, warning etc.
                    foreach (var diagnostic in diagnostics)
                    {
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }

            log = log.SelectMany(l => l.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None)).ToList();
            log = log.Select(l => $"// {l}").ToList();
            context.AddSource( "Log.txt", string.Join("\n",log));
        }

        private void ValidateStateMachine(WriteContext context)
        {
            var unnamedParameters = context.StateMachine.StateFragments
                .OfType<StateTransition>()
                .Where(t => t.Parameters.Any(p => p.Name == null))
                .Select(t => t.Parameters.First(p => p.Name == null))
                .ToArray();

            foreach (var unnamedParameter in unnamedParameters)
            {
                // We need to map the Antlr line indexing onto the Roslyn line indexing. They differ.
                var line = unnamedParameter.SourceLine - 1;
                var column = unnamedParameter.SourceColumn;

                var linePositionStart = new LinePosition(line, column);
                var linePositionEnd = new LinePosition(line, column);
                var linePositionSpan = new LinePositionSpan(linePositionStart, linePositionEnd);
                var textSpan = new TextSpan(column, 0);
                var location = Location.Create(context.OriginalFileName, textSpan, linePositionSpan);

                var diagnostic = Diagnostic.Create(_unnamedParameterRule, location, unnamedParameter.Type);

                context.Diagnostics.Add(diagnostic);
            }
        }

        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        private WriteContext CreateWriteContext(IndentedTextWriter writer, string originalFileName, List<string> log, List<Diagnostic> diagnostics, StateMachine stateMachine)
        {
            // We want to know all unique states defined in the diagram.
            var transitionStates = stateMachine.StateFragments.OfType<StateTransition>().SelectMany(t => new[] { t.From, t.To });
            var endTransitionStates = stateMachine.StateFragments.OfType<EndTransition>().SelectMany(t => new[] { t.From });
            var descriptionStates = stateMachine.StateFragments.OfType<StateDescription>().SelectMany(t => new[] { t.State });
            var allStates = transitionStates
                .Concat(endTransitionStates)
                .Concat(descriptionStates)
                .Distinct() // That is, of course without any doubles.
                .ToArray();

            // We want to know all unique triggers defined in the diagram.
            var transitionTriggers = stateMachine.StateFragments.OfType<StateTransition>().Select(t => t.Trigger);
            var endTransitionTriggers = stateMachine.StateFragments.OfType<EndTransition>().Select(t => t.Trigger);
            var allTriggers = transitionTriggers
                .Concat(endTransitionTriggers)
                .Distinct() // That is, of course without any doubles.
                .ToArray();

            // We want to know all unique transitions defined in the diagram.
            // That is, the transitions grouped by the trigger and unique sequence of parameters.
            var uniqueParameterTransitions = stateMachine.StateFragments
                .OfType<StateTransition>()
                .Select(t => new { Transition = t, ParametersAsKey = $"{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.First().Transition)
                .ToArray();

            return new WriteContext(writer, originalFileName, log, diagnostics, stateMachine, allStates, allTriggers, uniqueParameterTransitions);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
