// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine
{
    using Microsoft.CodeAnalysis;

    public static class GeneratorRule
    {
        private const string Prefix = "SL1";

        public static readonly DiagnosticDescriptor InvalidPlantUmlStateMachine = new
        (
            id: Prefix + "001",
            title: "PlantUml file is invalid",
            messageFormat: "PlantUml file is invalid: {0}",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor ParsingThrewException = new
        (
            id: Prefix + "002",
            title: "PlantUml parsing threw exception",
            messageFormat: "PlantUml parsing threw exception: '{0}' {1}",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor NoStartStatesDefined = new
        (
            id: Prefix + "003",
            title: "No start states defined",
            messageFormat: "No start states defined",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor UnnamedParameter = new
        (
            id: Prefix + "004",
            title: "Trigger definition uses unnamed parameters - Consider using named parameters",
            messageFormat: "Trigger definition uses unnamed parameters: '{0}' - Consider using named parameters",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor PlantUmlStateMachineProcessingThrowsException = new
        (
            id: Prefix + "005",
            title: "PlantUml processing throws exception",
            messageFormat: "PlantUml processing throws exception: '{0}' {1}",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor UnnamedTrigger = new
        (
            id: Prefix + "006",
            title: "State transition lacks a named trigger - Consider using named triggers",
            messageFormat: "State transition lacks a named trigger: '{0}' - Consider using named triggers (From -> To : Trigger)",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor SuperstateHasNoStartOrEntryTransitionDefined = new
        (
            id: Prefix + "007",
            title: "Superstate has no start or entry transitions defined",
            messageFormat: "Superstate {0} has no start or entry transition defined",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor SuperstateHasNoStopOrExitTransitionDefined = new
        (
            id: Prefix + "008",
            title: "Superstate has no stop or exit transitions defined",
            messageFormat: "Superstate {0} has no stop or exit transition defined",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor SuperstateHasBothUnnamedAndDirectTransitionsDefined = new
        (
            id: Prefix + "009",
            title: "Superstate has both unnamed start transitions as well as direct substate transitions defined",
            messageFormat: "Superstate {0} has both unnamed start transitions as well as direct substate transitions defined",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor SuperstateHasMultipleUnnamedStartTransitionsDefined = new
        (
            id: Prefix + "010",
            title: "Superstate can only have one single unnamed start transition defined",
            messageFormat: "Superstate {0} can only have one single unnamed start transition defined",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        public static readonly DiagnosticDescriptor DuplicateTriggers = new
        (
            id: Prefix + "011",
            title: "Trigger used multiple times in state",
            messageFormat: "Trigger '{0}' used {1} times in state '{2}'",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
    }
}
