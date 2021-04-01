namespace EtAlii.Generators.ML.Tests
{
    using Microsoft.ML.Data;

    /// <summary>
    /// Class to hold the fixed length feature vector. Used to define the
    /// column names used as output from the custom mapping action,
    /// </summary>
    public class FixedLength
    {
        /// <summary>
        /// This is a fixed length vector designated by VectorType attribute.
        /// </summary>
        [VectorType(SentimentAnalysis.FeatureLength)]
        public int[] Features { get; set; }
    }
}
