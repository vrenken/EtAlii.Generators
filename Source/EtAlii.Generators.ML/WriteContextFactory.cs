namespace EtAlii.Generators.ML
{
    using System;
    using System.CodeDom.Compiler;

    public class WriteContextFactory : IWriteContextFactory<object>
    {
        /// <summary>
        /// Create a context with commonly used instances and data that we can easily pass through the whole writing callstack.
        /// </summary>
        public WriteContext<object> Create(IndentedTextWriter writer, string originalFileName, object instance)
        {
            var namespaceDetails = new NamespaceDetails("NoNamespace", Array.Empty<string>());
            return new WriteContext(writer, originalFileName, instance, namespaceDetails);
        }
    }
}
