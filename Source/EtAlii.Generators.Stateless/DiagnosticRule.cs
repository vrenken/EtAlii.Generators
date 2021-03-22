// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using Microsoft.CodeAnalysis;

    public static class DiagnosticRule
    {
        public static readonly DiagnosticDescriptor InvalidPlantUmlStateMachine = new
        (
            id: "SL1001",
            title: "PlantUml file is invalid",
            messageFormat: "PlantUml file is invalid: {0}",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor PlantUmlStateMachineParsingThrowsException = new
        (
            id: "SL1002",
            title: "PlantUml parsing throws exception",
            messageFormat: "PlantUml parsing throws exception: '{0}' {1}",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor NoStartStatesDefined = new
        (
            id: "SL1003",
            title: "No start states defined",
            messageFormat: "No start states defined",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor UnnamedParameter = new
        (
            id: "SL1004",
            title: "Trigger definition uses unnamed parameters - Consider using named parameters",
            messageFormat: "Trigger definition uses unnamed parameters: '{0}' - Consider using named parameters",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor PlantUmlStateMachineProcessingThrowsException = new
        (
            id: "SL1005",
            title: "PlantUml processing throws exception",
            messageFormat: "PlantUml processing throws exception: '{0}' {1}",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor UnnamedTrigger = new
        (
            id: "SL1006",
            title: "State transition lacks a named trigger - Consider using named triggers",
            messageFormat: "State transition lacks a named trigger: '{0}' - Consider using named triggers (From -> To : Trigger)",
            category: "Code-Gen",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );
    }
}
