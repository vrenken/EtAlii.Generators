﻿namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using EtAlii.Generators.PlantUml;
    using Xunit;

    public class NamespaceWriterTypedTests
    {
        [Fact]
        public void NamespaceWriterTyped_Create()
        {
            // Arrange.
            var lifetime = new StatelessMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper();
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var eventArgsWriter = new EventArgsWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter, stateFragmentHelper);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter, lifetime, stateFragmentHelper);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, eventArgsWriter, instantiationWriter);

            // Act.
            var writer = new NamespaceWriter<StateMachine>(context => classWriter.Write(context));

            // Assert.
            Assert.NotNull(writer);
        }

        [Fact]
        public void NamespaceWriterTyped_Write_Simple()
        {
            // Arrange.
            var lifetime = new StatelessMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper();
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var eventArgsWriter = new EventArgsWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter, stateFragmentHelper);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter, lifetime, stateFragmentHelper);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, eventArgsWriter, instantiationWriter);
            var writer = new NamespaceWriter<StateMachine>(context => classWriter.Write(context));
            var originalFileName = "Test.puml";

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
                new Transition("First", "Second", new TransitionDetails("Continue", true), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(1,0, "")),
                new Transition("Second", "Third", new TransitionDetails("Continue", true), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(2,0, ""))
            };

            var allTransitions = Array.Empty<Transition>();

            var states = Array.Empty<State>();

            var allTriggers = Array.Empty<string>();

            var allSuperStates = Array.Empty<SuperState>();

            var stateMachine = new StateMachine(headers, settings, fragments, states, states, allTransitions, allTriggers, allSuperStates);

            using var stringWriter = new StringWriter();
            using var indentedTextWriter = new IndentedTextWriter(stringWriter);
            var writeContext = new WriteContextFactory().Create(indentedTextWriter, originalFileName, stateMachine);

            // Act.
            writer.Write(writeContext);

            // Assert.
            Assert.NotNull(writer);
            Assert.False(string.IsNullOrWhiteSpace(stringWriter.ToString()));
        }


        [Fact]
        public void NamespaceWriterTyped_Write_Internal()
        {
            // Arrange.
            var lifetime = new StatelessMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper();
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var eventArgsWriter = new EventArgsWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter, stateFragmentHelper);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter, lifetime, stateFragmentHelper);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, eventArgsWriter, instantiationWriter);
            var writer = new NamespaceWriter<StateMachine>(context => classWriter.Write(context));
            var originalFileName = "Test.puml";

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
                new Transition("First", "Second", new TransitionDetails("Continue", true), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(1,0, "")),
                new Transition("Second", "Third", new TransitionDetails("Continue", true), new TriggerDetails(false, Array.Empty<Parameter>()), new SourcePosition(2,0, "")),
                new Transition("Second", "Second", new TransitionDetails("Check", true), new TriggerDetails(true, new [] { new Parameter("string", "name", new SourcePosition(2,0, "")), new Parameter("Guid", "id", new SourcePosition(2,0, "")) }), new SourcePosition(2,0, ""))
            };

            var allTransitions = Array.Empty<Transition>();

            var states = Array.Empty<State>();

            var allTriggers = Array.Empty<string>();

            var allSuperStates = Array.Empty<SuperState>();

            var stateMachine = new StateMachine(headers, settings, fragments, states, states, allTransitions, allTriggers, allSuperStates);

            using var stringWriter = new StringWriter();
            using var indentedTextWriter = new IndentedTextWriter(stringWriter);
            var writeContext = new WriteContextFactory().Create(indentedTextWriter, originalFileName, stateMachine);

            // Act.
            writer.Write(writeContext);

            // Assert.
            Assert.NotNull(writer);
            Assert.False(string.IsNullOrWhiteSpace(stringWriter.ToString()));
        }
    }
}

