namespace EtAlii.Generators.EntityFrameworkCore
{
    using System;

    /// <summary>
    /// A code generator able to create EF Core source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    public class EntityModelWriterFactory : IWriterFactory<EntityModel>
    {
        public IWriter<EntityModel> Create()
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package dependency here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            var entityWriter = new EntityWriter();
            var dbContextWriter = new DbContextWriter();
            var writeEntities = new Action<WriteContext<EntityModel>>(context =>
            {
                foreach (var @class in context.Instance.Classes)
                {
                    entityWriter.Write(context, @class);
                }
            });

            var writeNamespaceContent = new Action<WriteContext<EntityModel>>(context =>
            {
                writeEntities(context);
                dbContextWriter.Write(context);
            });

            return new NamespaceWriter<EntityModel>(writeNamespaceContent);
        }
    }
}
