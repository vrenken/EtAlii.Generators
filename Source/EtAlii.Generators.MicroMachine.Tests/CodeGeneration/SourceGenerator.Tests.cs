namespace EtAlii.Generators.MicroMachine.Tests
{
    using Xunit;

    public class SourceGeneratorTests
    {
        [Fact]
        public void SourceGenerator_Create()
        {
            // Arrange.

            // Act.
            var generator = new SourceGenerator();

            // Assert.
            Assert.NotNull(generator);
        }
    }
}
