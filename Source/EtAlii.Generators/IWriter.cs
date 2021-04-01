// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public interface IWriter<in TInstance>
    {
        void Write(TInstance instance, IndentedTextWriter writer, string originalFileName, List<string> log, List<Diagnostic> writeDiagnostics);
    }
}
