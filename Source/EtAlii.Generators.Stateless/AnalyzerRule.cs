// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using Microsoft.CodeAnalysis;

    public static class AnalyzerRule
    {
        private const string Prefix = "SL2";

        public static DiagnosticDescriptor[] AllRules { get; }

        public static readonly DiagnosticDescriptor MethodNotImplemented = new
        (
            id: Prefix + "001",
            title: "State machine method not overridden",
            messageFormat: "State machine method '{0}' not overridden",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        static AnalyzerRule()
        {
            AllRules = new[] {MethodNotImplemented};
        }
    }
}
