namespace EtAlii.Generators.Stateless
{
    using System.CodeDom.Compiler;
    using EtAlii.Generators.PlantUml;
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
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly StateHierarchyBuilder _stateHierarchyBuilder;

        public SourceGenerator()
        {
            _lifetime = new StatelessMachineLifetime();
            _stateFragmentHelper = new StateFragmentHelper(_lifetime);
            _stateHierarchyBuilder = new StateHierarchyBuilder(_stateFragmentHelper, _lifetime);
        }

        protected override IParser<StateMachine> CreateParser() => new PlantUmlStateMachineParser(_lifetime, _stateHierarchyBuilder, _stateFragmentHelper);

        protected override IWriterFactory<StateMachine> CreateWriterFactory() => new StatelessWriterFactory(_lifetime, _stateFragmentHelper);

        protected override IValidator<StateMachine> CreateValidator() => new PlantUmlStateMachineValidator(_lifetime, _stateFragmentHelper);

        protected override string GetExtension() => ".puml";

        protected override string GetSourceItemGroup() => "StatelessFile";

        protected override DiagnosticDescriptor GetParsingExceptionRule() => GeneratorRule.PlantUmlStateMachineProcessingThrowsException;

        protected override WriteContext<StateMachine> CreateWriteContext(StateMachine instance, IndentedTextWriter writer, string originalFileName) =>  new WriteContextFactory().Create(writer, originalFileName, instance);
    }
}
