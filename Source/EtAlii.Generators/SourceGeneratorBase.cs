// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public abstract class SourceGeneratorBase<T> : ISourceGenerator
    {
        protected abstract IParser<T> CreateParser();
        protected abstract IWriter<T> CreateWriter();

        protected abstract IValidator<T> CreateValidator();

        protected abstract string GetExtension();

        protected abstract DiagnosticDescriptor GetParsingExceptionRule();

        public void Execute(GeneratorExecutionContext context)
        {
            // For testing and troubleshooting we'll use a simple list of strings that will be dumped to a file
            // at the end of the process.
            var log = new List<string>();

            // For actual troubleshooting of the source diagrams we use the Roslyn specific Diagnostics pattern.
            var diagnostics = new List<Diagnostic>();

            var extension = GetExtension();
            var parsingExceptionRule = GetParsingExceptionRule();

            // This code generator is only able to understand and parse the designated files.
            // Because of that we ignore everything except files with the configured extension.
            var additionalFiles = context.AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(extension, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var parser = CreateParser();
            var writer = CreateWriter();
            var validator = CreateValidator();

            foreach(var file in additionalFiles)
            {
                // First thing to do is parse the file and build an in-memory model.
                if (parser.TryParse(file, log, out var instance, out var parseDiagnostics))
                {
                    try
                    {
                        var originalFileName = Path.GetFileName(file.Path);
                        var fileName = Path.ChangeExtension(originalFileName, "Generated.cs");

                        validator.Validate(instance, originalFileName, diagnostics);

                        using var stringWriter = new StringWriter();
                        using var indentedWriter = new IndentedTextWriter(stringWriter);

                        writer.Write(instance, indentedWriter, originalFileName, log, diagnostics);

                        var content = stringWriter.ToString();
                        context.AddSource(fileName, content);
                    }
                    catch (Exception e)
                    {
                        log.Add($"File writing throws exception: {e.Message}");
                        log.Add($"{e.StackTrace}");

                        var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                        var diagnostic = Diagnostic.Create(parsingExceptionRule, location, e.Message, e.StackTrace);
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

        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to initialize.
        }
    }
}
