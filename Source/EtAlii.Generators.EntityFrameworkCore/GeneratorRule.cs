// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.EntityFrameworkCore
{
    using Microsoft.CodeAnalysis;

    public static class GeneratorRule
    {
        private const string Prefix = "EM1";

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

        public static readonly DiagnosticDescriptor PlantUmlProcessingThrowsException = new
        (
            id: Prefix + "003",
            title: "PlantUml processing throws exception",
            messageFormat: "PlantUml processing throws exception: '{0}' {1}",
            category: "EtAlii",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
    }
}
