// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// The central class responsible of validating both the PlantUML and Stateless specific requirements
    /// and express them using Roslyn Diagnostic instances.
    /// </summary>
    internal class StatelessPlantUmlValidator
    {
        public void Validate(WriteContext context, List<Diagnostic> diagnostics)
        {
            var startStates = context.AllTransitions
                .Where(t => t.From == SourceGenerator.BeginStateName)
                .ToArray();
            if (startStates.Length == 0)
            {
                var startStatesAsString = string.Join(", ", startStates.Select(s => s.To));
                var location = Location.Create(context.OriginalFileName, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(DiagnosticRule.NoStartStatesDefined, location, startStatesAsString);
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

                var diagnostic = Diagnostic.Create(DiagnosticRule.UnnamedParameter, location, unnamedParameter.Source.Text);

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

                var diagnostic = Diagnostic.Create(DiagnosticRule.UnnamedTrigger, location, transitionWithUnnamedTrigger.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }
    }
}
