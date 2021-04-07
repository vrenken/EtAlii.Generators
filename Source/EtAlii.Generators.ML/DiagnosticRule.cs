// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.ML
{
    using Microsoft.CodeAnalysis;

    public static class DiagnosticRule
    {
        private const string Prefix = "ML";

        public static readonly DiagnosticDescriptor InvalidPlantUmlStateMachine = new
        (
            id: Prefix + "1001",
            title: "GraphQL query file is invalid",
            messageFormat: "GraphQL query file is invalid: {0}",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor ParsingThrewException = new
        (
            id: Prefix + "1002",
            title: "GraphQL query parsing threw exception",
            messageFormat: "GraphQL query parsing threw exception: '{0}' {1}",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
    }
}
