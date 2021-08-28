// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.PlantUml.Tests
{
    using System.CodeDom.Compiler;
    using System.IO;
    using Xunit;

    public class NamespaceWriterTests
    {
        [Fact]
        public void NamespaceWriter_Create()
        {
            // Arrange.

            // Act.
            var writer = new NamespaceWriter<string>(_ => { });

            // Assert.
            Assert.NotNull(writer);
        }

        [Fact]
        public void NamespaceWriter_Write_Simple()
        {
            // Arrange.
            var writer = new NamespaceWriter<string>(context => context.Writer.Write(context.Instance));
            var originalFileName = "Test.txt";
            var namespaceDetails = new NamespaceDetails("RootNamespace.ChildNamespace", new []{ "System.Text" });
            var instance = "test 12";

            using var stringWriter = new StringWriter();
            using var indentedTriter = new IndentedTextWriter(stringWriter);
            var writeContext = new WriteContext<string>(indentedTriter, originalFileName, instance, namespaceDetails);

            // Act.
            writer.Write(writeContext);

            // Assert.
            Assert.NotNull(writer);
            Assert.False(string.IsNullOrWhiteSpace(stringWriter.ToString()));
        }
    }
}
