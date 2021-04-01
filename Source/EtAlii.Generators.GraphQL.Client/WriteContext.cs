// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.GraphQL.Client
{
    using System.CodeDom.Compiler;

    public class WriteContext : WriteContextBase<object>
    {
        public WriteContext(IndentedTextWriter writer, string originalFileName, object instance)
            : base(writer, originalFileName, instance)
        {
        }
    }
}
