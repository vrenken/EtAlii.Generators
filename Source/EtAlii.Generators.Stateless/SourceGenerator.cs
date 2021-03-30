namespace EtAlii.Generators.Stateless
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// A code generator able to create Stateless source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    [Generator]
    public class SourceGenerator : SourceGeneratorBase<StateMachine>
    {
        public const string StateMachineType = "global::Stateless.StateMachine<State, Trigger>";
        public const string BeginStateName = "_Begin";
        public const string EndStateName = "_End";

        private readonly NamespaceWriter _namespaceWriter;
        private readonly StatelessPlantUmlParser _parser;
        private readonly StatelessPlantUmlValidator _validator;
        public SourceGenerator()
        : base(".puml", DiagnosticRule.PlantUmlStateMachineProcessingThrowsException)
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

            _parser = new StatelessPlantUmlParser();
            _validator = new StatelessPlantUmlValidator();
        }

        protected override bool TryParseFile(AdditionalText file, List<string> log, out StateMachine stateMachine, out Diagnostic[] parseDiagnostics)
        {
            return _parser.TryParsePlantUml(file, log, out stateMachine, out parseDiagnostics);
        }

        protected override void WriteContent(StateMachine stateMachine, IndentedTextWriter writer, string originalFileName, List<string> log, List<Diagnostic> writeDiagnostics)
        {

            // If there is no classname defined in the diagram we'll need to come up with one ourselves.
            if (!stateMachine.Settings.OfType<ClassNameSetting>().Any())
            {
                // Let's use a C# safe subset of the characters in the filename.
                var classNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(originalFileName), "[^a-zA-Z0-9_]", "");
                stateMachine.AddSettings(new ClassNameSetting(classNameFromFileName));
            }

            var writeContext = new WriteContextFactory().Create(writer, originalFileName, log, stateMachine);

            _validator.Validate(writeContext, writeDiagnostics);

            _namespaceWriter.Write(writeContext);
        }
    }
}
