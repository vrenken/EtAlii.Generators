namespace EtAlii.Generators.EntityFrameworkCore
{
    public class EntityWriter
    {
        public void Write(WriteContext<EntityModel> context, Class @class)
        {
            var prefix = "";
            var postfix = !string.IsNullOrWhiteSpace(context.Instance.EntityName)
                ? $" : {context.Instance.EntityName}"
                : "";
            context.Writer.WriteLine($"public {prefix}class {@class.Name}{postfix}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            foreach (var property in @class.Properties)
            {
                context.Writer.WriteLine($"public {property.Type} {property.Name} {{ get; set; }}");
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }
    }
}
