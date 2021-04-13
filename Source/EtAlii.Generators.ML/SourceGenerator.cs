namespace EtAlii.Generators.ML
{
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
        protected override IParser<object> CreateParser() => new GraphQLQueryParser();

        protected override IWriterFactory<object> CreateWriterFactory() => new GraphQLQueryWriterFactory();

        protected override IValidator<object> CreateValidator() => new GraphQLQueryValidator();

        protected override string GetExtension() => ".graphql";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => DiagnosticRule.ParsingThrewException;

        protected override WriteContext<object> CreateWriteContext(object instance, IndentedTextWriter writer, string originalFileName, List<string> log)
        {
            return new WriteContextFactory().Create(writer, originalFileName, log, instance);
        }
    }
}
