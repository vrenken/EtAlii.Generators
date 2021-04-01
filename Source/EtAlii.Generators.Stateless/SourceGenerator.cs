namespace EtAlii.Generators.Stateless
{
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
        protected override IParser<StateMachine> CreateParser() => new StatelessPlantUmlParser();

        protected override IWriter<StateMachine> CreateWriter() => new StatelessWriter();

        protected override IValidator<StateMachine> CreateValidator() => new StatelessPlantUmlValidator();

        protected override string GetExtension() => ".puml";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => DiagnosticRule.PlantUmlStateMachineProcessingThrowsException;
    }
}
