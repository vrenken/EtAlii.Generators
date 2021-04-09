namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;

    public class WriteContextFactory : IWriteContextFactory<EntityModel>
    {
        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext<EntityModel> Create(IndentedTextWriter writer, string originalFileName, List<string> log, EntityModel model)
        {
            var usings = new[] {"System", "Microsoft.EntityFrameworkCore" }.Concat(model.Usings).ToArray();
            var namespaceDetails = new NamespaceDetails(model.Namespace, usings);
            return new WriteContext(writer, originalFileName, model, namespaceDetails);
        }
    }
}
