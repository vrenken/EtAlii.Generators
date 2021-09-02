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
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public SourceGenerator()
        {
            _lifetime = new MicroStateMachineLifetime();
            _stateFragmentHelper = new StateFragmentHelper(_lifetime);
        }

        protected override IParser<StateMachine> CreateParser() => new PlantUmlStateMachineParser(new MicroStateMachineLifetime());

        protected override IWriterFactory<StateMachine> CreateWriterFactory() => new MicroMachineWriterFactory(_lifetime, _stateFragmentHelper);

        protected override IValidator<StateMachine> CreateValidator() => new PlantUmlStateMachineValidator(_lifetime, _stateFragmentHelper);

        protected override string GetExtension() => ".puml";

        protected override string GetSourceItemGroup() => "MicroMachine";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlStateMachineProcessingThrowsException;

        protected override WriteContext<StateMachine> CreateWriteContext(StateMachine instance, IndentedTextWriter writer, string originalFileName, List<string> log) =>  new WriteContextFactory(_stateFragmentHelper).Create(writer, originalFileName, log, instance);
    }
}
