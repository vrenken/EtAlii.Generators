namespace EtAlii.Generators.ML.Tests
{
    using System.Linq;
    using Xunit;

    public class ImageClassificationTests
    {
        [Fact]
        public void ImageClassification_Import()
        {
            // Arrange.
            var classification = new ImageClassification();
            classification.Init();
            var testData = ImageNetData.ReadFromCsv(classification.DataLocation, classification.ImagesFolder);

            // Act.
            var results = testData
                .Select(sample => new {Sample = sample, Result = classification.Predict(sample)})
                .ToArray();

            // Assert.
            Assert.NotEmpty(results);
            foreach (var result in results)
            {
                if (result.Sample.ImagePath.EndsWith("coffeepot4.jpg"))
                {
                    Assert.Equal("pitcher", result.Result.PredictedLabel);
                }
                else
                {
                    Assert.Equal(result.Sample.ExpectedLabel, result.Result.PredictedLabel);
                }
            }
        }
    }
}
