// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.ML
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// The central class responsible of validating the query requirements
    /// and express them using Roslyn Diagnostic instances.
    /// </summary>
    public class MachineLearningQueryValidator : IValidator<object>
    {
        public void Validate(object instance, string originalFileName, List<Diagnostic> diagnostics)
        {
            // We don't know what is needed to validate the ML model.
        }
    }
}
