﻿namespace EtAlii.Generators.GraphQL.Client
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
        protected override IParser<object> CreateParser() => new GraphQLQueryParser();

        protected override IWriterFactory<object> CreateWriterFactory() => new GraphQLQueryWriterFactory();

        protected override IValidator<object> CreateValidator() => new GraphQLQueryValidator();

        protected override string GetExtension() => ".graphql";

        protected override string GetSourceItemGroup() => "GraphQLQuery";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => DiagnosticRule.ParsingThrewException;

        protected override WriteContext<object> CreateWriteContext(object instance, IndentedTextWriter writer, string originalFileName)
        {
            return new WriteContextFactory().Create(writer, originalFileName, instance);
        }
    }
}
