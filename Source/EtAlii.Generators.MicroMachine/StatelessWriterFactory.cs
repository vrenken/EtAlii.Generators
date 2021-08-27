namespace EtAlii.Generators.MicroMachine
{
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
    public class MicroMachineWriterFactory : IWriterFactory<StateMachine>
    {
        public IWriter<StateMachine> Create()
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var eventArgsWriter = new EventArgsWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, eventArgsWriter, instantiationWriter);
            return new NamespaceWriter<StateMachine>(context => classWriter.Write(context));
        }
    }
}
