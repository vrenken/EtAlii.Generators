namespace EtAlii.Generators.EntityFrameworkCore
{
    public class EntityWriter
    {
        public void Write(WriteContext<EntityModel> context, Class @class)
        {
            var prefix = "";
            context.Writer.WriteLine($"public {prefix} class {@class.Name}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }
    }
}
