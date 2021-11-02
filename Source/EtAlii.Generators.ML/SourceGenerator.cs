namespace EtAlii.Generators.ML
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// A code generator able to create typed GraphQL queries from .graphql files.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [Generator]
    public class SourceGenerator : SourceGeneratorBase<object>
    {
        protected override IParser<object> CreateParser() => new MachineLearningQueryParser();

        protected override IWriterFactory<object> CreateWriterFactory() => new MachineLearningQueryWriterFactory();

        protected override IValidator<object> CreateValidator() => new MachineLearningQueryValidator();

        protected override string GetExtension() => ".ml";

        protected override string GetSourceItemGroup() => "MLFile";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => DiagnosticRule.ParsingThrewException;

        protected override WriteContext<object> CreateWriteContext(object instance, IndentedTextWriter writer, string originalFileName)
        {
            return new WriteContextFactory().Create(writer, originalFileName, instance);
        }
    }
}
