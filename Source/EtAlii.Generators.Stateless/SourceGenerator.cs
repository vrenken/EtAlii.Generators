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
        public const string BeginStateName = "_Begin";
        public const string EndStateName = "_End";

        public void Execute(GeneratorExecutionContext context)
        {
            // For testing and troubleshooting we'll use a simple list of strings that will be dumped to a file
            // at the end of the process.
            var log = new List<string>();

            // For actual troubleshooting of the source diagrams we use the Roslyn specific Diagnostics pattern.
            var diagnostics = new List<Diagnostic>();

            // This code generator is only able to understand and parse PlantUml files.
            // Because of that we ignore everything except files with the .puml extension.
            var plantUmlFiles = context.AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".puml", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach(var file in plantUmlFiles)
            {
                // First thing to do is parse the .puml file and build an in-memory model.
                if (TryParsePlantUml(file, log, out var stateMachine, out var parseDiagnostics))
                {
                    try
                    {
                        var originalFileName = Path.GetFileName(file.Path);

                        // If there is no classname defined in the diagram we'll need to come up with one ourselves.
                        if (!stateMachine.Settings.OfType<ClassNameSetting>().Any())
                        {
                            // Let's use a C# safe subset of the characters in the filename.
                            var classNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(originalFileName), "[^a-zA-Z0-9_]", "");
                            stateMachine.AddSettings(new ClassNameSetting(classNameFromFileName));
                        }

                        var fileName = Path.ChangeExtension(originalFileName, "Generated.cs");

                        using var stringWriter = new StringWriter();
                        using var writer = new IndentedTextWriter(stringWriter);
                        var writeContext = CreateWriteContext(writer, originalFileName, log, stateMachine);

                        ValidateStateMachine(writeContext, diagnostics);

                        WriteNamespace(writeContext);

                        var content = stringWriter.ToString();
                        context.AddSource(fileName, content);
                    }
                    catch (Exception e)
                    {
                        log.Add($"File writing throws exception: {e.Message}");
                        log.Add($"{e.StackTrace}");

                        var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                        var diagnostic = Diagnostic.Create(_plantUmlStateMachineProcessingThrowsExceptionRule, location, e.Message, e.StackTrace);
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

            log = log.SelectMany(l => l.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None)).ToList();
            log = log.Select(l => $"// {l}").ToList();
            context.AddSource( "Log.txt", string.Join("\n",log));
        }

        private void ValidateStateMachine(WriteContext context, List<Diagnostic> diagnostics)
        {
            var startStates = context.AllTransitions
                .Where(t => t.From == BeginStateName)
                .ToArray();
            if (startStates.Length == 0)
            {
                var startStatesAsString = string.Join(", ", startStates.Select(s => s.To));
                var location = Location.Create(context.OriginalFileName, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(_noStartStatesDefinedRule, location, startStatesAsString);
                diagnostics.Add(diagnostic);
            }

            var unnamedParameters = context.AllTransitions
                .Where(t => t.Parameters.Any(p => !p.HasName))
                .Select(t => t.Parameters.First(p => !p.HasName))
                .ToArray();

            foreach (var unnamedParameter in unnamedParameters)
            {
                // We need to map the Antlr line indexing onto the Roslyn line indexing. They differ.
                var line = unnamedParameter.Source.Line - 1;
                var column = unnamedParameter.Source.Column;

                var linePositionStart = new LinePosition(line, column);
                var linePositionEnd = new LinePosition(line, column);
                var linePositionSpan = new LinePositionSpan(linePositionStart, linePositionEnd);
                var textSpan = new TextSpan(column, 0);
                var location = Location.Create(context.OriginalFileName, textSpan, linePositionSpan);

                var diagnostic = Diagnostic.Create(_unnamedParameterRule, location, unnamedParameter.Source.Text);

                diagnostics.Add(diagnostic);
            }

            var transitionsWithUnnamedTrigger = context.AllTransitions
                .Where(t => !t.HasConcreteTriggerName)
                .ToArray();

            foreach (var transitionWithUnnamedTrigger in transitionsWithUnnamedTrigger)
            {
                // We need to map the Antlr line indexing onto the Roslyn line indexing. They differ.
                var line = transitionWithUnnamedTrigger.Source.Line - 1;
                var column = transitionWithUnnamedTrigger.Source.Column;

                var linePositionStart = new LinePosition(line, column);
                var linePositionEnd = new LinePosition(line, column);
                var linePositionSpan = new LinePositionSpan(linePositionStart, linePositionEnd);
                var textSpan = new TextSpan(column, 0);
                var location = Location.Create(context.OriginalFileName, textSpan, linePositionSpan);

                var diagnostic = Diagnostic.Create(_unnamedTriggerRule, location, transitionWithUnnamedTrigger.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        private WriteContext CreateWriteContext(IndentedTextWriter writer, string originalFileName, List<string> log, StateMachine stateMachine)
        {
            var allTransitions = stateMachine.StateFragments
                .OfType<StateTransition>()
                .ToArray();

            log.Add("Transitions found:");
            log.AddRange(allTransitions.Select(t =>
            {
                var parameters = t.Parameters.Any()
                    ? $" ({string.Join(", ", t.Parameters.Select(p => $"{p.Type} {p.Name}".Trim()))}) "
                    : " ";

                var asyncPrefix = t.IsAsync ? "async " : "";
                var stereoType = t.IsAsync || t.Parameters.Any()
                    ? $" << {asyncPrefix}{parameters}>> "
                    : " ";
                return $"- {t.From} -> {t.To}{stereoType}: {t.Trigger}";
            }));

            // We want to know all unique states defined in the diagram.
            var transitionStates = allTransitions.SelectMany(t => new[] { t.From, t.To });
            var descriptionStates = stateMachine.StateFragments.OfType<StateDescription>().SelectMany(t => new[] { t.State });
            var allStates = transitionStates
                .Concat(descriptionStates)
                .Where(s => s != null)
                .OrderBy(s => s)
                .Distinct() // That is, of course without any doubles.
                .ToArray();

            log.Add("States found:");
            log.AddRange(allStates.Select(s => $"- {s}"));

            // We want to know all unique triggers defined in the diagram.
            var transitionTriggers = allTransitions.Select(t => t.Trigger);
            var allTriggers = transitionTriggers
                .OrderBy(t => t)
                .Distinct() // That is, of course without any doubles.
                .ToArray();

            log.Add("Triggers found:");
            log.AddRange(allTriggers.Select(t => $"- {t}"));

            // We want to know all unique transitions defined in the diagram.
            // That is, the transitions grouped by the trigger and unique sequence of parameters.
            var uniqueParameterTransitions = allTransitions
                .Select(t => new { Transition = t, ParametersAsKey = $"{t.Trigger}{string.Join(", ", t.Parameters.Select(p => p.Type))}" })
                .GroupBy(item => item.ParametersAsKey)
                .Select(g => g.First().Transition)
                .ToArray();

            return new WriteContext(writer, originalFileName, stateMachine, allStates, allTriggers, allTransitions, uniqueParameterTransitions);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to initialize.
        }
    }
}
