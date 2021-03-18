namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    internal class PlantUmlErrorListener : IAntlrErrorListener<object>
    {
        private readonly string _fileName;
        private readonly List<Diagnostic> _diagnostics;
        public IReadOnlyCollection<Diagnostic> Diagnostics { get; }

        public PlantUmlErrorListener(string fileName)
        {
            _fileName = fileName;
            _diagnostics = new();
            Diagnostics = new ReadOnlyCollection<Diagnostic>(_diagnostics);
        }
        public void SyntaxError(TextWriter output, IRecognizer recognizer, object offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e)

        {
            // We need to map the Antlr line indexing onto the Roslyn line indexing. They differ.
            line -= 1;

            var linePositionStart = new LinePosition(line, charPositionInLine);
            var linePositionEnd = new LinePosition(line, charPositionInLine);
            var linePositionSpan = new LinePositionSpan(linePositionStart, linePositionEnd);
            var textSpan = new TextSpan(charPositionInLine, 0);
            var location = Location.Create(_fileName, textSpan, linePositionSpan);


            var diagnostic = Diagnostic.Create(SourceGenerator.InvalidPlantUmlStateMachineRule, location, msg);

            _diagnostics.Add(diagnostic);
            output.WriteLine($"line {line}:{charPositionInLine} {msg}");
        }
    }
}
