namespace EtAlii.Generators.ML.Tests
{
    using Xunit;

    public class SentimentModelTests
    {
        [Fact]
        public void SentimentModel_Import()
        {
            // Arrange.
            var program = new SentimentProgram();

            // Act.
            program.Main(null);

            // Act.
        }
    }
}
