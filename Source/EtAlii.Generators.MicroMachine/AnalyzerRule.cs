// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine
{
    using Microsoft.CodeAnalysis;

    public static class AnalyzerRule
    {
        private const string Prefix = "SL2";

        public static DiagnosticDescriptor[] AllRules { get; }

        public static readonly DiagnosticDescriptor MethodNotImplemented = new
        (
            id: Prefix + "001",
            title: "State machine does not implement state handling method",
            messageFormat: "State machine '{0}' does not implement state handling method '{1}'",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        static AnalyzerRule()
        {
            AllRules = new[] {MethodNotImplemented};
        }
    }
}
