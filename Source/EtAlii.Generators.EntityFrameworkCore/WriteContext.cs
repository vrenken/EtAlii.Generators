// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.CodeDom.Compiler;

    public class WriteContext : WriteContext<EntityModel>
    {
        public WriteContext(IndentedTextWriter writer, string originalFileName, EntityModel instance, NamespaceDetails namespaceDetails)
            : base(writer, originalFileName, instance, namespaceDetails)
        {
        }
    }
}
