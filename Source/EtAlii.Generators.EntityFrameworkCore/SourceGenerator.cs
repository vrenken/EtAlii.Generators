namespace EtAlii.Generators.EntityFrameworkCore
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
    public class SourceGenerator : SourceGeneratorBase<EntityModel>
    {
        protected override IParser<EntityModel> CreateParser() => new EntityModelPlantUmlParser();

        protected override IWriter<EntityModel> CreateWriter() => new EntityModelWriter();

        protected override IValidator<EntityModel> CreateValidator() => new EntityModelPlantUmlValidator();

        protected override string GetExtension() => ".puml";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlProcessingThrowsException;
    }
}
