// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System.Linq;
    using Microsoft.CodeAnalysis;

    internal static class SourceGeneratorContextExtensions
    {
        private const string _sourceItemGroupMetadata = "build_metadata.AdditionalFiles.SourceItemGroup";

        public static AdditionalText[] GetAdditionalFilesWithSourceItemGroup(this GeneratorExecutionContext context, string sourceItemGroup)
        {
            return context
                    .AdditionalFiles
                    .Where(f =>
                    {
                        context.AnalyzerConfigOptions
                                   .GetOptions(f)
                                   .TryGetValue(_sourceItemGroupMetadata, out var sig);
                        return sig == sourceItemGroup;
                    })
                    .ToArray();
        }
    }
}
