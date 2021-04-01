// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public interface IValidator<in TInstance>
    {
        void Validate(TInstance instance, string originalFileName, List<Diagnostic> diagnostics);
    }
}
