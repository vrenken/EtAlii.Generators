namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;
    using Xunit;

    public class StatelessPlantUmlParserTests
    {
        [Fact]
        public void StatelessPlantUmlParser_Create()
        {
            // Arrange.

            // Act.
            var parser = new StatelessPlantUmlParser();

            // Assert.
            Assert.NotNull(parser);
        }

        [Fact]
        public void StatelessPlantUmlParser_TryParse_Valid()
        {

            // Arrange.
            var parser = new StatelessPlantUmlParser();
            var log = new List<string>();

            var text = @"@startuml
'stateless namespace EtAlii.Generators.Stateless.Tests
'stateless class MyFancyStateMachineBase
'stateless generate partial
'stateless using System.Text

[*] -> State1 << (string name) >> : Start
State1 -> State2 << (string name) >> : Continue
State2 -> State2 << async (string name) >> : Check
State2 -up-> State3 : Continue
@enduml";
            var file = new TestAdditionalTextFile(text, "Test.puml");

            // Act.
            var result = parser.TryParsePlantUml(file, log, out var stateMachine, out var diagnostics);

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
            var parser = new StatelessPlantUmlParser();
            var log = new List<string>();

            var text = @"@startuml
'stateless namespace EtAlii.Generators.Stateless.Tests
'stateless class MyFancyStateMachineBase
'stateless generate partial
'stateless using System.Text

[*] -> State1 << (string name) >> : Start
State1 -> State2 << (string name) >INVALID> : Continue
State2 -> State2 << async (string name) >> : Check
State2 -up-> State3 : Continue
@enduml";
            var file = new TestAdditionalTextFile(text, "Test.puml");

            // Act.
            var result = parser.TryParsePlantUml(file, log, out var stateMachine, out var diagnostics);

            // Assert.
            Assert.False(result);
            Assert.Null(stateMachine);
            Assert.NotEmpty(diagnostics);
        }
    }
}
