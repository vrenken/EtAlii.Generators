// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using System.CodeDom.Compiler;

    public class WriteContext : WriteContext<StateMachine>
    {
        public WriteContext(IndentedTextWriter writer, string originalFileName, StateMachine instance, NamespaceDetails namespaceDetails)
            : base(writer, originalFileName, instance, namespaceDetails)
        {
        }
    }
}
