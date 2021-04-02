namespace EtAlii.Generators.ML.Tests
{
    using Microsoft.ML.Data;

    public class ImageNetPrediction
    {
        [ColumnName(InceptionSettings.OutputTensorName)]
        public float[] PredictedLabels;
    }
}
