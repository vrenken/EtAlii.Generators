// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using Microsoft.CodeAnalysis;

    public interface IParser<TInstance>
    {
        bool TryParse(AdditionalText file, out TInstance instance, out Diagnostic[] diagnostics);
    }
}
