namespace EtAlii.Generators.Stateless.Tests
{
    using EtAlii.Generators.PlantUml;
    using Xunit;

    public class ParameterConverterTests
    {
        [Fact]
        public void ParameterConverter_Create()
        {
            // Arrange.

            // Act.
            var converter = new ParameterConverter();

            // Assert.
            Assert.NotNull(converter);
        }

        [Fact]
        public void ParameterConverter_ToCamelCase_From_UpperCase()
        {
            // Arrange.
            var converter = new ParameterConverter();

            // Act.
            var output = converter.ToCamelCase("TEST");

            // Assert.
            Assert.Equal("tEST", output);
        }

        [Fact]
        public void ParameterConverter_ToParameter_With_Name()
        {
            // Arrange.
            var converter = new ParameterConverter();
            var parameter = new Parameter("Type", "Name", new SourcePosition(0, 0, ""));

            // Act.
            var output = converter.ToParameterName(parameter);

            // Assert.
            Assert.Equal("Name", output);
        }

        [Fact]
        public void ParameterConverter_ToParameter_Without_Name()
        {
            // Arrange.
            var converter = new ParameterConverter();
            var parameter = new Parameter("Type", null, new SourcePosition(0, 0, ""));

            // Act.
            var output = converter.ToParameterName(parameter);

            // Assert.
            Assert.Equal("type", output);
        }

    }
}
