namespace EtAlii.Generators.ML
{
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

        protected override IParser<object> CreateParser() => new GraphQLQueryParser();

        protected override IWriter<object> CreateWriter() => new GraphQLQueryWriter();

        protected override IValidator<object> CreateValidator() => new GraphQLQueryValidator();

        protected override string GetExtension() => ".graphql";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => DiagnosticRule.ParsingThrewException;
    }
}
