namespace EtAlii.Generators.EntityFrameworkCore
{
    public class DbContextWriter
    {
        public void Write(WriteContext<EntityModel> context)
        {
            var hasDbContextName = !string.IsNullOrWhiteSpace(context.Instance.DbContextName);
            if (hasDbContextName)
            {
                context.Writer.WriteLine();
                context.Writer.WriteLine($"public class {context.Instance.DbContextName} : global::Microsoft.EntityFrameworkCore.DbContext");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;

                foreach (var @class in context.Instance.Classes)
                {
                    var propertyName = @class.Mapping?.Name ?? @class.Name;
                    context.Writer.WriteLine($"public virtual DbSet<{@class.Name}> {propertyName} {{ get; set; }}");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();
            }
        }
    }
}
