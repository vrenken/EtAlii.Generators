namespace EtAlii.Generators
{
    using System;

    public class NamespaceWriter<TInstance> : IWriter<TInstance>
    {
        private readonly Action<WriteContext<TInstance>> _writeContent;

        public NamespaceWriter(Action<WriteContext<TInstance>> writeContent)
        {
            _writeContent = writeContent;
        }

        public void Write(WriteContext<TInstance> context)
        {
            context.Writer.WriteLine($"// Remark: this file was auto-generated based on '{context.OriginalFileName}'.");
            context.Writer.WriteLine("// Any changes will be overwritten the next time the file is generated.");
            context.Writer.WriteLine($"namespace {context.NamespaceDetails.Name}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            foreach (var @using in context.NamespaceDetails.Usings)
            {
                context.Writer.WriteLine($"using {@using};");
            }

            context.Writer.WriteLine();

            _writeContent(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
