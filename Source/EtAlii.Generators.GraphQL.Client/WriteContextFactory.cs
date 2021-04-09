namespace EtAlii.Generators.GraphQL.Client
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    public class WriteContextFactory : IWriteContextFactory<object>
    {
        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext<object> Create(IndentedTextWriter writer, string originalFileName, List<string> log, object instance)
        {
            var namespaceDetails = new NamespaceDetails("NoNamespace", Array.Empty<string>());
            return new WriteContext(writer, originalFileName, instance, namespaceDetails);
        }
    }
}
