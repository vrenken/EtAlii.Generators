// Copyright (c) Peter Vrenken. All rights reserved. See the license in https://github.com/vrenken/EtAlii.Ubigia

namespace EtAlii.Generators.Stateless
{
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
    }
}
