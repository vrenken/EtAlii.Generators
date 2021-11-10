namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    /// <summary>
    /// A code generator able to create MicroMachine source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    public class MicroMachineWriterFactory : IWriterFactory<StateMachine>
    {
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public MicroMachineWriterFactory(IStateMachineLifetime lifetime, StateFragmentHelper stateFragmentHelper)
        {
            _lifetime = lifetime;
            _stateFragmentHelper = stateFragmentHelper;
        }

        public IWriter<StateMachine> Create()
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            var parameterConverter = new ParameterConverter();
            var stateFragmentHelper = new StateFragmentHelper(_lifetime);
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var toDifferentStateMethodChainBuilder = new ToDifferentStateMethodChainBuilder(stateFragmentHelper);
            var toSameStateMethodChainBuilder = new ToSameStateMethodChainBuilder(stateFragmentHelper, toDifferentStateMethodChainBuilder);
            var methodChainBuilder = new MethodChainBuilder(toDifferentStateMethodChainBuilder, toSameStateMethodChainBuilder, _stateFragmentHelper, _lifetime);
            var transitionMethodWriter = new TransitionMethodWriter(methodChainBuilder);
            var transitionClassWriter = new TransitionClassWriter();
            var triggerMethodWriter = new TriggerMethodWriter(parameterConverter, transitionConverter, _lifetime, _stateFragmentHelper, methodChainBuilder);
            var triggerClassWriter = new TriggerClassWriter(parameterConverter, transitionConverter);
            var choicesWriter = new ChoicesWriter(triggerMethodWriter);
            var stateMachineClassWriter = new ClassWriter(enumWriter, transitionMethodWriter, triggerMethodWriter, triggerClassWriter, transitionClassWriter, _stateFragmentHelper, parameterConverter, choicesWriter);
            return new NamespaceWriter<StateMachine>(context => stateMachineClassWriter.Write(context));
        }
    }
}
