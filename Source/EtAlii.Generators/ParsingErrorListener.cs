namespace EtAlii.Generators
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public class ParsingErrorListener : IAntlrErrorListener<object>
    {
        private readonly string _fileName;
        private readonly DiagnosticDescriptor _parsingExceptionDiagnosticRule;
        private readonly List<Diagnostic> _diagnostics;
        public IReadOnlyCollection<Diagnostic> Diagnostics { get; }

        public ParsingErrorListener(string fileName, DiagnosticDescriptor parsingExceptionDiagnosticRule)
        {
            _fileName = fileName;
            _parsingExceptionDiagnosticRule = parsingExceptionDiagnosticRule;
            _diagnostics = new List<Diagnostic>();
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


            var diagnostic = Diagnostic.Create(_parsingExceptionDiagnosticRule, location, msg);

            _diagnostics.Add(diagnostic);
            output.WriteLine($"line {line}:{charPositionInLine} {msg}");
        }
    }
}
