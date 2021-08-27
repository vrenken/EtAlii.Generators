// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Text;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public class TestAdditionalTextFile : AdditionalText
    {
        private readonly string _text;
        private readonly string _path;

        public TestAdditionalTextFile(string text, string path)
        {
            _text = text;
            _path = path;
        }
        public override SourceText GetText(CancellationToken cancellationToken = new()) => SourceText.From(_text, Encoding.Default);
        public override string Path => _path;
    }
}
