namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using Xunit;

    public class StatelessPlantUmlValidatorTests
    {
        [Fact]
        public void StatelessPlantUmlValidator_Create()
        {
            // Arrange.

            // Act.
            var parser = new StatelessPlantUmlValidator();

            // Assert.
            Assert.NotNull(parser);
        }

        [Fact]
        public void StatelessPlantUmlValidator_Validate()
        {
            // Arrange.
            var originalFileName = "Test.puml";
            var parser = new StatelessPlantUmlValidator();
            var headers = new[]
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
                new Transition(null, "Third", new TransitionDetails("Start", false, Array.Empty<Parameter>(), false), new SourcePosition(1,0, "")),
                new Transition("First", "Second", new TransitionDetails("Continue", false, Array.Empty<Parameter>(), true), new SourcePosition(2,0, "")),
                new Transition("Second", "Third", new TransitionDetails("Continue", false, Array.Empty<Parameter>(), true), new SourcePosition(3,0, ""))
            };
            var stateMachine = new StateMachine(headers, settings, fragments);
            using var stringWriter = new StringWriter();
            using var indentedTriter = new IndentedTextWriter(stringWriter);
            var diagnostics = new List<Diagnostic>();

            // Act.
            parser.Validate(stateMachine, originalFileName, diagnostics);

            // Assert.
            Assert.NotEmpty(diagnostics);
        }
    }
}
