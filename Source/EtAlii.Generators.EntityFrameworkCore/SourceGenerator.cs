﻿namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// A code generator able to create EF Core source code from PlantUML diagrams.
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

        protected override string GetSourceItemGroup() => "EfCoreModel";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlProcessingThrowsException;

        protected override WriteContext<EntityModel> CreateWriteContext(EntityModel instance, IndentedTextWriter writer, string originalFileName)
        {
            return new WriteContextFactory().Create(writer, originalFileName, instance);
        }
    }
}
