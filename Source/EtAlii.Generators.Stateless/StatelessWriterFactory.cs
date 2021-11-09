namespace EtAlii.Generators.Stateless
{
    using EtAlii.Generators.PlantUml;

    /// <summary>
    /// A code generator able to create Stateless source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    public class StatelessWriterFactory : IWriterFactory<StateMachine>
    {
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public StatelessWriterFactory(IStateMachineLifetime lifetime, StateFragmentHelper stateFragmentHelper)
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
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter, _stateFragmentHelper);
            var eventArgsWriter = new EventArgsWriter(methodWriter, _stateFragmentHelper);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter, _stateFragmentHelper);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter, _lifetime, _stateFragmentHelper);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, eventArgsWriter, instantiationWriter);
            return new NamespaceWriter<StateMachine>(context => classWriter.Write(context));
        }
    }
}
