namespace EtAlii.Generators.Stateless.Tests
{
    using System.IO;
    using Xunit;

    public class PlantUmlErrorListenerTests
    {
        [Fact]
        public void PlantUmlErrorListener_Create()
        {
            // Arrange.
            var fileName = "Test.puml";

            // Act.
            var listener = new PlantUmlErrorListener(fileName, DiagnosticRule.InvalidPlantUmlStateMachine);

            // Assert.
            Assert.NotNull(listener);
        }

        [Fact]
        public void PlantUmlErrorListener_Raise_SyntaxError()
        {
            // Arrange.
            var fileName = "Test.puml";
            var listener = new PlantUmlErrorListener(fileName, DiagnosticRule.InvalidPlantUmlStateMachine);
            var writer = new StringWriter();

            // Act.
            listener.SyntaxError(writer, null, null, 1, 2, "Test exception", null);

            // Assert.
            Assert.NotNull(listener);
            Assert.NotEmpty(listener.Diagnostics);
            Assert.StartsWith("line", writer.ToString());
        }
    }
}
