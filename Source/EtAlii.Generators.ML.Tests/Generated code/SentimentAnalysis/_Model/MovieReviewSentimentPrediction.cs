namespace EtAlii.Generators.ML.Tests
{
    using Microsoft.ML.Data;

    /// <summary>
    /// Class to contain the output values from the transformation.
    /// </summary>
    public class MovieReviewSentimentPrediction
    {
        [VectorType(2)]
        public float[] Prediction { get; set; }
    }
}
