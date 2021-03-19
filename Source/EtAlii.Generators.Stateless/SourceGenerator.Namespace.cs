namespace EtAlii.Generators.Stateless
{
    public partial class SourceGenerator
    {
        private void WriteNamespace(WriteContext context)
        {
            context.Writer.WriteLine($"// Remark: this file was auto-generated based on '{context.OriginalFileName}'.");
            context.Writer.WriteLine("// Any changes will be overwritten the next time the file is generated.");
            context.Writer.WriteLine($"namespace {context.StateMachine.Namespace}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("using System;");
            context.Writer.WriteLine("using System.Threading.Tasks;");
            context.Writer.WriteLine("using Stateless;");

            foreach (var @using in context.StateMachine.Usings)
            {
                context.Writer.WriteLine($"using {@using};");
            }

            context.Writer.WriteLine();
            WriteClass(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
