namespace EtAlii.Generators.Stateless
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
    public class SourceGenerator : SourceGeneratorBase<StateMachine>
    {
        protected override IParser<StateMachine> CreateParser() => new StatelessPlantUmlParser();

        protected override IWriterFactory<StateMachine> CreateWriterFactory() => new StatelessWriterFactory();

        protected override IValidator<StateMachine> CreateValidator() => new StatelessPlantUmlValidator();

        protected override string GetSourceItemGroup() => "StatelessFile";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlStateMachineProcessingThrowsException;

        protected override WriteContext<StateMachine> CreateWriteContext(StateMachine instance, IndentedTextWriter writer, string originalFileName, List<string> log)
        {
            return new WriteContextFactory().Create(writer, originalFileName, log, instance);
        }
    }
}
