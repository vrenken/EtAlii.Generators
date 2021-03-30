namespace EtAlii.Generators.GraphQL.Client
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// A code generator able to create typed GraphQL queries from .graphql files.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Generator]
    public class SourceGenerator : SourceGeneratorBase<object>
    {
        public const string StateMachineType = "global::Stateless.StateMachine<State, Trigger>";
        public const string BeginStateName = "_Begin";
        public const string EndStateName = "_End";

        private readonly NamespaceWriter _namespaceWriter;
        private readonly StatelessPlantUmlParser _plantUmlParser;
        private readonly StatelessPlantUmlValidator _plantUmlValidator;
        public SourceGenerator()
        : base(".graphql", DiagnosticRule.PlantUmlStateMachineProcessingThrowsException)
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, instantiationWriter);
            _namespaceWriter = new NamespaceWriter(classWriter);

            _plantUmlParser = new StatelessPlantUmlParser();
            _plantUmlValidator = new StatelessPlantUmlValidator();
        }

        protected override bool TryParseFile(AdditionalText file, List<string> log, out object instance, out Diagnostic[] parseDiagnostics) => throw new NotImplementedException();

        protected override void WriteContent(object instance, IndentedTextWriter writer, string originalFileName, List<string> log, List<Diagnostic> writeDiagnostics) => throw new NotImplementedException();
    }
}
