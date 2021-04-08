﻿namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// A code generator able to create Stateless source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    [Generator]
    public class EntityModelWriter : IWriter<EntityModel>
    {
        //private readonly NamespaceWriter _namespaceWriter;

        public EntityModelWriter()
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            // var parameterConverter = new ParameterConverter();
            // var transitionConverter = new TransitionConverter(parameterConverter);
            // var enumWriter = new EnumWriter();
            // var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            // var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            // var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            // var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, instantiationWriter);
            // _namespaceWriter = new NamespaceWriter(classWriter);
        }

        public void Write(EntityModel model, IndentedTextWriter writer, string originalFileName, List<string> log, List<Diagnostic> writeDiagnostics)
        {
            // If there is no DbContext name defined in the diagram we'll need to come up with one ourselves.
            if (!model.Settings.OfType<DbContextNameSetting>().Any())
            {
                // Let's use a C# safe subset of the characters in the filename.
                var dbContextNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(originalFileName), "[^a-zA-Z0-9_]", "");
                model.AddSettings(new DbContextNameSetting(dbContextNameFromFileName));
            }

            var writeContext = new WriteContextFactory().Create(writer, originalFileName, log, model);

            //_namespaceWriter.Write(writeContext);
        }
    }
}
