// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.GraphQL.Client
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class GraphQLQueryWriter : IWriter<object>
    {
        public void Write(object instance, IndentedTextWriter writer, string originalFileName, List<string> log, List<Diagnostic> writeDiagnostics)
        {
            writer.Write("Test");
        }
    }
}
