// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    public interface IWriteContextFactory<T>
    {
        WriteContext<T> Create(IndentedTextWriter writer, string originalFileName, List<string> log, T instance);
    }
}
