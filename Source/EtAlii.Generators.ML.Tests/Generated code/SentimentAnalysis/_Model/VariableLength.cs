namespace EtAlii.Generators.ML.Tests
{
    using Microsoft.ML.Data;

    /// <summary>
    /// Class to hold the variable length feature vector. Used to define the
    /// column names used as input to the custom mapping action.
    /// </summary>
    public class VariableLength
    {
        /// <summary>
        /// This is a variable length vector designated by VectorType attribute.
        /// Variable length vectors are produced by applying operations such as 'TokenizeWords' on strings
        /// resulting in vectors of tokens of variable lengths.
        /// </summary>
        [VectorType]
        public int[] VariableLengthFeatures { get; set; }
    }
}
