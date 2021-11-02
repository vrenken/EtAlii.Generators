// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System.CodeDom.Compiler;

    public interface IWriteContextFactory<T>
    {
        WriteContext<T> Create(IndentedTextWriter writer, string originalFileName, T instance);
    }
}
