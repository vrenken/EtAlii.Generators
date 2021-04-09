namespace EtAlii.Generators.EntityFrameworkCore
{
    using System;
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
        private readonly IWriter<EntityModel> _namespaceWriter;

        public EntityModelWriter()
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package dependency here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            var entityWriter = new EntityWriter();

            var writeEntities = new Action<WriteContext<EntityModel>>(context =>
            {
                foreach (var @class in context.Instance.Classes)
                {
                    entityWriter.Write(context, @class);
                }
            });
            _namespaceWriter = new NamespaceWriter<EntityModel>(writeEntities);
        }

        public void Write(WriteContext<EntityModel> context)
        {
            _namespaceWriter.Write(context);
        }
    }
}
