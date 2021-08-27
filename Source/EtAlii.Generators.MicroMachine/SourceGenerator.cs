namespace EtAlii.Generators.MicroMachine
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using EtAlii.Generators.PlantUml;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// A code generator able to create MicroMachine source code from PlantUML diagrams.
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
        protected override IParser<StateMachine> CreateParser() => new MicroMachinePlantUmlParser();

        protected override IWriterFactory<StateMachine> CreateWriterFactory() => new MicroMachineWriterFactory();

        protected override IValidator<StateMachine> CreateValidator() => new MicroMachinePlantUmlValidator();

        protected override string GetExtension() => ".puml";

        protected override string GetSourceItemGroup() => "MicroMachineFile";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlStateMachineProcessingThrowsException;

        protected override WriteContext<StateMachine> CreateWriteContext(StateMachine instance, IndentedTextWriter writer, string originalFileName, List<string> log)
        {
            return new WriteContextFactory().Create(writer, originalFileName, log, instance);
        }
    }
}
