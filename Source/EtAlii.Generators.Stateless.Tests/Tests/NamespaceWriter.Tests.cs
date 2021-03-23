namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
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

        [Fact]
        public void NamespaceWriter_Write_Simple()
        {
            // Arrange.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, instantiationWriter);
            var writer = new NamespaceWriter(classWriter);
            var originalFileName = "Test.puml";
            var log = new List<string>();

            var headers = new Header[]
            {
                new Header("This is a stub header")
            };
            var settings = new Setting[]
            {
                new GeneratePartialClassSetting(true),
                new NamespaceSetting("EtAlii.TestNamespace"),
                new ClassNameSetting("TestClass"),
                new UsingSetting("EtAlii.TestNamespace.NestedNamespace")
            };

            var fragments = new StateFragment[]
            {
                new Transition("First", "Second", new TransitionDetails("Continue", false, Array.Empty<Parameter>(), true), new SourcePosition(0,0, "")),
                new Transition("Second", "Third", new TransitionDetails("Continue", false, Array.Empty<Parameter>(), true), new SourcePosition(0,0, ""))
            };

            var stateMachine = new StateMachine(headers, settings, fragments);

            using var stringWriter = new StringWriter();
            using var indentedTriter = new IndentedTextWriter(stringWriter);
            var writeContext = new WriteContextFactory().Create(indentedTriter, originalFileName, log, stateMachine);

            // Act.
            writer.Write(writeContext);

            // Assert.
            Assert.NotNull(writer);
            Assert.False(string.IsNullOrWhiteSpace(stringWriter.ToString()));
        }
    }
}
