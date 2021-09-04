// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// The central class responsible of validating both the PlantUML and EntityFramework specific requirements
    /// and express them using Roslyn Diagnostic instances.
    /// </summary>
    public class EntityModelPlantUmlValidator : IValidator<EntityModel>
    {
        public void Validate(EntityModel instance, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            // We don't know what is needed to validate the EF model.
        }
    }
}
