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

    public abstract partial class SourceGeneratorBase<T> : ISourceGenerator
    {
        private const string SourceItemGroupMetadata = "build_metadata.AdditionalFiles.SourceItemGroup";

        protected abstract IParser<T> CreateParser();
        protected abstract IWriterFactory<T> CreateWriterFactory();
        protected abstract WriteContext<T> CreateWriteContext(T instance, IndentedTextWriter writer, string originalFileName);
        protected abstract IValidator<T> CreateValidator();

        protected abstract string GetSourceItemGroup();

        protected abstract string GetExtension();

        protected abstract DiagnosticDescriptor GetParsingExceptionRule();

        public void Execute(GeneratorExecutionContext context)
        {
            using (Correlation.Begin(CorrelationType.SourceGeneration))
            // For testing and troubleshooting we'll use localised Seq logging. This should become deactivated outside of the primary development system.
            SetupLogging();

            // For actual troubleshooting of the source diagrams we use the Roslyn specific Diagnostics pattern.
            var diagnostics = new List<Diagnostic>();

            var extension = GetExtension();
            var sourceItemGroup = GetSourceItemGroup();
            var parsingExceptionRule = GetParsingExceptionRule();

            // This code generator is only able to understand and parse the designated files.
            // Because of that we ignore everything except files with the configured extension.
            var additionalFiles = context.AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(extension, StringComparison.OrdinalIgnoreCase))
                .Where(f =>
                {
                    try
                    {
                        var options = context.AnalyzerConfigOptions.GetOptions(f);
                        if (options.TryGetValue(SourceItemGroupMetadata, out var sourceItemGroupMetadata))
                        {
                            _log.Information("Found {SourceItemGroup} {SourceItemGroupMetadata}", Path.GetFileName(f.Path), sourceItemGroupMetadata);
                            return sourceItemGroupMetadata.Equals(sourceItemGroup, StringComparison.OrdinalIgnoreCase);
                        }
                        else
                        {
                            _log.Error("Failed finding {SourceItemGroup}", Path.GetFileName(f.Path));
                        }
                    }
                    catch (Exception e)
                    {
                        _log.Fatal(e, "Exception while finding SourceItemGroup");
                    }
                    return false;
                })
                .ToArray();

            var parser = CreateParser();
            var writerFactory = CreateWriterFactory();
            var writer = writerFactory.Create();
            var validator = CreateValidator();

            foreach(var file in additionalFiles)
            {
                // First thing to do is parse the file and build an in-memory model.
                if (parser.TryParse(file, out var instance, out var parseDiagnostics))
                {
                    try
                    {
                        var originalFileName = Path.GetFileName(file.Path);
                        var fullPathToFile = file.Path;
                        var fileName = Path.ChangeExtension(originalFileName, "Generated.cs");

                        validator.Validate(instance, fullPathToFile, diagnostics);

                        using var stringWriter = new StringWriter();
                        using var indentedWriter = new IndentedTextWriter(stringWriter);

                        _log.Information("Creating WriteContext");
                        var writeContext = CreateWriteContext(instance, indentedWriter, originalFileName);
                        writer.Write(writeContext);

                        var content = stringWriter.ToString();
                        context.AddSource(fileName, content);
                    }
                    catch (Exception e)
                    {
                        _log.Fatal(e, "File writing threw an exception");

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
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to initialize.
        }
    }
}
