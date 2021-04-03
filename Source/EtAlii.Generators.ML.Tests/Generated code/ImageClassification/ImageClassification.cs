namespace EtAlii.Generators.ML.Tests
{
    using System.IO;
    using System;
    using System.Linq;
    using Microsoft.ML;

    public class ImageClassification
    {
        public string DataLocation => _dataLocation;
        private readonly string _dataLocation;
        public string ImagesFolder => _imagesFolder;
        private readonly string _imagesFolder;
        private readonly string _modelLocation;
        private readonly MLContext _mlContext;
        private PredictionEngine<ImageNetData, ImageNetPrediction> _predictionEngine;
        private readonly string[] _labels;

        private const int ImageHeight = 224;
        private const int ImageWidth = 224;
        private const float Mean = 117;
        private const bool ChannelsLast = true;

        // for checking tensor names, you can use tools like Netron,
        // which is installed by Visual Studio AI Tools

        // input tensor name
        public const string InputTensorName = "input";

        // output tensor name
        public const string OutputTensorName = "softmax2";
        public ImageClassification()
        {
            //var assetsRelativePath = @"../../../assets";

            var assemblyFolderPath = Path.GetDirectoryName(GetType().Assembly.Location);
            var rootFolder = Path.Combine(assemblyFolderPath!, @"Generated code\ImageClassification");
            //var assetsPath = GetAbsolutePath(assetsRelativePath);

            _dataLocation = Path.Combine(rootFolder, @"assets\inputs\images\tags.tsv");
            _imagesFolder = Path.Combine(rootFolder, @"assets\inputs\images");
            _modelLocation = Path.Combine(rootFolder, @"assets\inputs\inception\tensorflow_inception_graph.pb");
            var labelsLocation = Path.Combine(rootFolder, @"assets\inputs\inception\imagenet_comp_graph_label_strings.txt");

            _labels = File.ReadAllLines(labelsLocation);

            _mlContext = new MLContext();
        }

        public void Init()
        {
            var data = _mlContext.Data.LoadFromTextFile<ImageNetData>(_dataLocation, hasHeader: true);

            var pipeline = _mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: _imagesFolder, inputColumnName: nameof(ImageNetData.ImagePath))
                .Append(_mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: ImageWidth, imageHeight: ImageHeight, inputColumnName: "input"))
                .Append(_mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: ChannelsLast, offsetImage: Mean))
                .Append(_mlContext.Model.LoadTensorFlowModel(_modelLocation).
                    ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2" },
                        inputColumnNames: new[] { InputTensorName }, addBatchDimensionInput:true));

            ITransformer model = pipeline.Fit(data);

            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ImageNetData, ImageNetPrediction>(model);
        }

        public ImageNetDataProbability Predict(ImageNetData sample)
        {
            var predictions = _predictionEngine.Predict(sample).PredictedLabels;
            var max = predictions.Max();
            var index = predictions.AsSpan().IndexOf(max);

            var result = new ImageNetDataProbability
            {
                PredictedLabel = _labels[index],
                Probability = max,
            };

            return result;
        }
    }
}
