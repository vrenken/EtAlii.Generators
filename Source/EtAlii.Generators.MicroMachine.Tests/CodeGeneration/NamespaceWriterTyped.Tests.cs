﻿namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using EtAlii.Generators.PlantUml;
    using Xunit;

    public class NamespaceWriterTypedTests
    {
        [Fact]
        public void NamespaceWriterTyped_Create()
        {
            // Arrange.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var triggerClassWriter = new TriggerClassWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, triggerClassWriter, instantiationWriter);

            // Act.
            var writer = new NamespaceWriter<StateMachine>(context => classWriter.Write(context));

            // Assert.
            Assert.NotNull(writer);
        }

        [Fact]
        public void NamespaceWriterTyped_Write_Simple()
        {
            // Arrange.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var triggerClassWriter = new TriggerClassWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, triggerClassWriter, instantiationWriter);
            var writer = new NamespaceWriter<StateMachine>(context => classWriter.Write(context));
            var originalFileName = "Test.puml";
            var log = new List<string>();

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


        [Fact]
        public void NamespaceWriterTyped_Write_Internal()
        {
            // Arrange.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter<StateMachine>();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var triggerClassWriter = new TriggerClassWriter(methodWriter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, triggerClassWriter, instantiationWriter);
            var writer = new NamespaceWriter<StateMachine>(context => classWriter.Write(context));
            var originalFileName = "Test.puml";
            var log = new List<string>();

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

