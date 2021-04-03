namespace EtAlii.Generators.ML.Tests
{
    using Microsoft.ML.Data;

    public class ImageNetPrediction
    {
        [ColumnName(ImageClassification.OutputTensorName)]
        public float[] PredictedLabels;
    }
}
