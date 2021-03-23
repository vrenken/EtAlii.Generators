namespace EtAlii.Generators.Stateless.Tests
{
    using Xunit;

    public class NamespaceWriterTests
    {
        [Fact]
        public void NamespaceWriter_Create()
        {
            // Arrange.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, instantiationWriter);

            // Act.
            var writer = new NamespaceWriter(classWriter);

            // Assert.
            Assert.NotNull(writer);
        }
    }
}
