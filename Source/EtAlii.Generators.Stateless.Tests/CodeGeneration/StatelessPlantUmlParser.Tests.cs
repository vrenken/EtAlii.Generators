﻿namespace EtAlii.Generators.Stateless.Tests
{
    using EtAlii.Generators.PlantUml;
    using Xunit;

    public class StatelessPlantUmlParserTests
    {
        [Fact]
        public void StatelessPlantUmlParser_Create()
        {
            // Arrange.
            var lifetime = new StatelessMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper();
            var stateHierarchyBuilder = new StateHierarchyBuilder(stateFragmentHelper, lifetime);

            // Act.
            var parser = new PlantUmlStateMachineParser(lifetime, stateHierarchyBuilder, stateFragmentHelper);

            // Assert.
            Assert.NotNull(parser);
        }

        [Fact]
        public void StatelessPlantUmlParser_TryParse_Valid()
        {

            // Arrange.
            var lifetime = new StatelessMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper();
            var stateHierarchyBuilder = new StateHierarchyBuilder(stateFragmentHelper, lifetime);
            var parser = new PlantUmlStateMachineParser(lifetime, stateHierarchyBuilder, stateFragmentHelper);

            var text = @"@startuml
'namespace EtAlii.Generators.Stateless.Tests
'class MyFancyStateMachineBase
'generate partial
'using System.Text

note This is a state machine
' And this is a comment

[*] -> State1 << (string name) >> : Start
State1 -> State2 << (string name) >> : Continue
State2 -> State2 << async (string[] names) >> : Check
State2 -up-> State3 << (string) >> : Continue
State3 -up-> State4 : Continue
State4 -> [*]
State4 : This is the final state
@enduml";
            var file = new TestAdditionalTextFile(text, "Test.puml");

            // Act.
            var result = parser.TryParse(file, out var stateMachine, out var diagnostics);

            // Assert.
            Assert.True(result);
            Assert.NotNull(stateMachine);
            Assert.Equal("EtAlii.Generators.Stateless.Tests", stateMachine.Namespace);
            Assert.Equal("MyFancyStateMachineBase", stateMachine.ClassName);
            Assert.NotNull(diagnostics);
            Assert.Empty(diagnostics);
        }


        [Fact]
        public void StatelessPlantUmlParser_TryParse_Invalid()
        {
            // Arrange.
            var lifetime = new StatelessMachineLifetime();
            var stateFragmentHelper = new StateFragmentHelper();
            var stateHierarchyBuilder = new StateHierarchyBuilder(stateFragmentHelper, lifetime);
            var parser = new PlantUmlStateMachineParser(lifetime, stateHierarchyBuilder, stateFragmentHelper);

            var text = @"@startuml
'namespace EtAlii.Generators.Stateless.Tests
'class MyFancyStateMachineBase
'generate partial
'using System.Text

[*] -> State1 << (string name) >> : Start
State1 -> State2 << (string name) >INVALID> : Continue
State2 -> State2 << async (string name) >> : Check
State2 -up-> State3 : Continue
@enduml";
            var file = new TestAdditionalTextFile(text, "Test.puml");

            // Act.
            var result = parser.TryParse(file, out var stateMachine, out var diagnostics);

            // Assert.
            Assert.False(result);
            Assert.Null(stateMachine);
            Assert.NotEmpty(diagnostics);
        }
    }
}
