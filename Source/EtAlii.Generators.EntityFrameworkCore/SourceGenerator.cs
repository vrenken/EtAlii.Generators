namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
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

        protected override IWriterFactory<EntityModel> CreateWriterFactory() => new EntityModelWriterFactory();

        protected override IValidator<EntityModel> CreateValidator() => new EntityModelPlantUmlValidator();

        protected override string GetExtension() => ".puml";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlProcessingThrowsException;

        protected override WriteContext<EntityModel> CreateWriteContext(EntityModel instance, IndentedTextWriter writer, string originalFileName, List<string> log)
        {
            return new WriteContextFactory().Create(writer, originalFileName, log, instance);
        }
    }
}
