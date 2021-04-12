namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.Linq;

    public class EntityWriter
    {
        private bool IsRelationProperty(WriteContext<EntityModel> context, string @class, string property)
        {
            var result = false;
            result |= context.Instance.Relations.Any(r => r.From == @class && r.Mapping.FromProperty == property);
            result |= context.Instance.Relations.Any(r => r.To == @class && r.Mapping.ToProperty == property);
            return result;
        }
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
                var isRelationProperty = IsRelationProperty(context, @class.Name, property.Name);
                var isMarkedAsCollection = property.Type.EndsWith("[]");
                if (isRelationProperty && isMarkedAsCollection)
                {
                    var type = property.Type.Substring(0, property.Type.Length - 2);
                    context.Writer.WriteLine($"public IList<{type}> {property.Name} {{ get; private set; }} = new List<{type}>();");
                }
                else
                {
                    context.Writer.WriteLine($"public {property.Type} {property.Name} {{ get; set; }}");
                }
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }
    }
}
