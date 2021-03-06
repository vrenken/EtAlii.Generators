// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using Antlr4.Runtime;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public class SourcePosition
    {
        public int Line { get; }

        public int Column { get; }

        public string Text { get; }

        public SourcePosition(int line, int column, string text)
        {
            Line = line;
            Column = column;
            Text = text;
        }

        public Location ToLocation(string fileName)
        {
            // We need to map the Antlr line indexing onto the Roslyn line indexing. They differ.
            var line = Line - 1;
            var column = Column;

            var linePositionStart = new LinePosition(line, column);
            var linePositionEnd = new LinePosition(line, column);
            var linePositionSpan = new LinePositionSpan(linePositionStart, linePositionEnd);
            var textSpan = new TextSpan(column, 0);
            return Location.Create(fileName, textSpan, linePositionSpan);
        }

        public static SourcePosition FromContext(ParserRuleContext context)
        {
            return new(context.Start.Line, context.Start.Column, context.GetText());
        }
    }
}
