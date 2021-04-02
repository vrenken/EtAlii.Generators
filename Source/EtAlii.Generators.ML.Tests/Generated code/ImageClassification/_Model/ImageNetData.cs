namespace EtAlii.Generators.ML.Tests
{
    using Microsoft.ML.Data;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ImageNetData
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string ExpectedLabel;

        public static IEnumerable<ImageNetData> ReadFromCsv(string file, string folder)
        {
            return File.ReadAllLines(file)
             .Select(x => x.Split('\t'))
             .Select(x => new ImageNetData { ImagePath = Path.Combine(folder, x[0]), ExpectedLabel = x[1] } );
        }
    }
}
