// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.ML.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.ML;
    using Microsoft.ML.Data;

    public class SentimentAnalysis
    {
        private readonly MLContext _context;
        private PredictionEngine<MovieReview, MovieReviewSentimentPrediction> _predictionEngine;

        public const int FeatureLength = 600;

        // private static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "sentiment_model");
        private const string _modelPath = @"Generated code\SentimentAnalysis";

        public SentimentAnalysis()
        {
            // Create MLContext to be shared across the model creation workflow objects
            _context = new MLContext();

        }
        public void Init()
        {

            // Dictionary to encode words as integers.
            var lookupMap = _context.Data.LoadFromTextFile(Path.Combine(_modelPath, "imdb_word_index.csv"),
                columns: new[]
                {
                    new TextLoader.Column("Words", DataKind.String, 0),
                    new TextLoader.Column("Ids", DataKind.Int32, 1),
                },
                separatorChar: ','
            );

            // The model expects the input feature vector to be a fixed length vector.
            // This action resizes the variable length array generated by the lookup map
            // to a fixed length vector. If there are less than 600 words in the sentence,
            // the remaining indices will be filled with zeros. If there are more than
            // 600 words in the sentence, then the array is truncated at 600.
            void ResizeFeaturesAction(VariableLength s, FixedLength f)
            {
                var features = s.VariableLengthFeatures;
                Array.Resize(ref features, FeatureLength);
                f.Features = features;
            }

            // Load the TensorFlow model.
            var tensorFlowModel = _context.Model.LoadTensorFlowModel(_modelPath);

            var schema = tensorFlowModel.GetModelSchema();
            Console.WriteLine(" =============== TensorFlow Model Schema =============== ");
            var featuresType = (VectorDataViewType)schema["Features"].Type;
            Console.WriteLine($"Name: Features, Type: {featuresType.ItemType.RawType}, Size: ({featuresType.Dimensions[0]})");
            var predictionType = (VectorDataViewType)schema["Prediction/Softmax"].Type;
            Console.WriteLine($"Name: Prediction/Softmax, Type: {predictionType.ItemType.RawType}, Size: ({predictionType.Dimensions[0]})");

            IEstimator<ITransformer> pipeline =
                // Split the text into individual words
                _context.Transforms.Text.TokenizeIntoWords("TokenizedWords", "ReviewText")
                    // Map each word to an integer value. The array of integer makes up the input features.
                    .Append(_context.Transforms.Conversion.MapValue("VariableLengthFeatures", lookupMap, lookupMap.Schema["Words"], lookupMap.Schema["Ids"], "TokenizedWords"))
                    // Resize variable length vector to fixed length vector.
                    .Append(_context.Transforms.CustomMapping((Action<VariableLength, FixedLength>) ResizeFeaturesAction, "Resize"))
                    // Passes the data to TensorFlow for scoring
                    .Append(tensorFlowModel.ScoreTensorFlowModel("Prediction/Softmax", "Features"))
                    // Retrieves the 'Prediction' from TensorFlow and and copies to a column
                    .Append(_context.Transforms.CopyColumns("Prediction", "Prediction/Softmax"));

            // Create an executable model from the estimator pipeline
            var dataView = _context.Data.LoadFromEnumerable(new List<MovieReview>());
            var model = pipeline.Fit(dataView);

            _predictionEngine = _context.Model.CreatePredictionEngine<MovieReview, MovieReviewSentimentPrediction>(model);
        }

        public MovieReviewSentimentPrediction Predict(MovieReview review)
        {
            // Predict with TensorFlow pipeline.
            return _predictionEngine.Predict(review);
        }
    }
}