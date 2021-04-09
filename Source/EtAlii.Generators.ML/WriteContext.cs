// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.ML
{
    using System.CodeDom.Compiler;

    public class WriteContext : WriteContext<object>
    {
        public WriteContext(IndentedTextWriter writer, string originalFileName, object instance, NamespaceDetails namespaceDetails)
            : base(writer, originalFileName, instance, namespaceDetails)
        {
        }
    }
}
