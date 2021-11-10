namespace EtAlii.Generators.PlantUml.Tests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using EtAlii.Generators.PlantUml;
    using Microsoft.CodeAnalysis;
    using Xunit;

    public class PlantUmlStateMachineValidatorTests
    {
        [Fact]
        public void PlantUmlStateMachineValidator_Create()
        {
            // Arrange.
            var lifetime = new PlantUmlTestMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper(lifetime);

            // Act.
            var parser = new PlantUmlStateMachineValidator(lifetime, stateFragmentHelper);

            // Assert.
            Assert.NotNull(parser);
        }

        [Fact]
        public void PlantUmlStateMachineValidator_Validate()
        {
            // Arrange.
            var lifetime = new PlantUmlTestMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper(lifetime);
            var fullPathToFile =  "Test.puml";
            var parser = new PlantUmlStateMachineValidator(lifetime, stateFragmentHelper);
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
                new Transition(null, "Third", new TransitionDetails("Start", false), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(1,0, "")),
                new Transition("First", "Second", new TransitionDetails("Continue", true), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(2,0, "")),
                new Transition("Second", "Third", new TransitionDetails("Continue", true), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(3,0, ""))
            };

            var allTransitions = Array.Empty<Transition>();

            var states = Array.Empty<State>();

            var allTriggers = Array.Empty<string>();

            var allSuperStates = Array.Empty<SuperState>();

            var stateMachine = new StateMachine(headers, settings, fragments, states, states, allTransitions, allTriggers, allSuperStates);
            using var stringWriter = new StringWriter();
            using var indentedTriter = new IndentedTextWriter(stringWriter);
            var diagnostics = new List<Diagnostic>();

            // Act.
            parser.Validate(stateMachine, fullPathToFile, diagnostics);

            // Assert.
            Assert.NotEmpty(diagnostics);
        }
    }
}
