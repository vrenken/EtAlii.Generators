namespace EtAlii.Generators.ML.Tests
{
    using Xunit;

    public class SentimentAnalysisTests
    {
        [Fact]
        public void SentimentAnalysis_Import()
        {
            // Arrange.
            var review = new MovieReview { ReviewText = "this film is really good" };
            var analysis = new SentimentAnalysis();
            analysis.Init();

            // Act.
            var result = analysis.Predict(review);

            // Assert.
            Assert.Equal(2, result.Prediction.Length);
            Assert.Equal(0.512078702f, result.Prediction[1], 6);
        }
    }
}
