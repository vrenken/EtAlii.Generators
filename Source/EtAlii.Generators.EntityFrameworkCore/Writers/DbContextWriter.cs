namespace EtAlii.Generators.EntityFrameworkCore
{
    public class DbContextWriter
    {
        public void Write(WriteContext<EntityModel> context)
        {
            var hasDbContextName = !string.IsNullOrWhiteSpace(context.Instance.DbContextName);
            if (hasDbContextName)
            {
                context.Writer.WriteLine();
                context.Writer.WriteLine($"public class {context.Instance.DbContextName} : global::Microsoft.EntityFrameworkCore.DbContext");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;

                foreach (var @class in context.Instance.Classes)
                {
                    var propertyName = @class.Mapping?.Name ?? @class.Name;
                    context.Writer.WriteLine($"public virtual DbSet<{@class.Name}> {propertyName} {{ get; set; }}");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();

                context.Writer.WriteLine($"public interface I{context.Instance.DbContextName}");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;

                foreach (var @class in context.Instance.Classes)
                {
                    var propertyName = @class.Mapping?.Name ?? @class.Name;
                    context.Writer.WriteLine($"DbSet<{@class.Name}> {propertyName} {{ get; }}");
                    context.Writer.WriteLine();
                }

                context.Writer.WriteLine("/// <summary>");
                context.Writer.WriteLine("///     <para>");
                context.Writer.WriteLine("///         Saves all changes made in this context to the database.");
                context.Writer.WriteLine("///     </para>");
                context.Writer.WriteLine("///     <para>");
                context.Writer.WriteLine("///         This method will automatically call <see cref=\"ChangeTracker.DetectChanges\" /> to discover any");
                context.Writer.WriteLine("///         changes to entity instances before saving to the underlying database. This can be disabled via");
                context.Writer.WriteLine("///         <see cref=\"ChangeTracker.AutoDetectChangesEnabled\" />.");
                context.Writer.WriteLine("///     </para>");
                context.Writer.WriteLine("///     <para>");
                context.Writer.WriteLine("///         Multiple active operations on the same context instance are not supported.  Use 'await' to ensure");
                context.Writer.WriteLine("///         that any asynchronous operations have completed before calling another method on this context.");
                context.Writer.WriteLine("///     </para>");
                context.Writer.WriteLine("/// </summary>");
                context.Writer.WriteLine("/// <param name=\"cancellationToken\"> A <see cref=\"CancellationToken\" /> to observe while waiting for the task to complete. </param>");
                context.Writer.WriteLine("/// <returns>");
                context.Writer.WriteLine("///     A task that represents the asynchronous save operation. The task result contains the");
                context.Writer.WriteLine("///     number of state entries written to the database.");
                context.Writer.WriteLine("/// </returns>");
                context.Writer.WriteLine("/// <exception cref=\"DbUpdateException\">");
                context.Writer.WriteLine("///     An error is encountered while saving to the database.");
                context.Writer.WriteLine("/// </exception>");
                context.Writer.WriteLine("/// <exception cref=\"DbUpdateConcurrencyException\">");
                context.Writer.WriteLine("///     A concurrency violation is encountered while saving to the database.");
                context.Writer.WriteLine("///     A concurrency violation occurs when an unexpected number of rows are affected during save.");
                context.Writer.WriteLine("///     This is usually because the data in the database has been modified since it was loaded into memory.");
                context.Writer.WriteLine("/// </exception>");
                context.Writer.WriteLine("Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);");
                context.Writer.WriteLine();

                context.Writer.WriteLine("/// <summary>");
                context.Writer.WriteLine("/// Access to the change tracking entity entry of the underlying DbContext.");
                context.Writer.WriteLine("/// </summary>");
                context.Writer.WriteLine("/// <param name=\"entity\">The entity to get the entry for.</param>");
                context.Writer.WriteLine("/// <returns>The entry for the given entity.</returns>");
                context.Writer.WriteLine("EntityEntry Entry([NotNull] object entity);");
                context.Writer.WriteLine();

                context.Writer.WriteLine("/// <summary>");
                context.Writer.WriteLine("///     <para>");
                context.Writer.WriteLine("///         Saves all changes made in this context to the database.");
                context.Writer.WriteLine("///     </para>");
                context.Writer.WriteLine("///     <para>");
                context.Writer.WriteLine("///         This method will automatically call <see cref=\"ChangeTracker.DetectChanges\" /> to discover any");
                context.Writer.WriteLine("///         changes to entity instances before saving to the underlying database. This can be disabled via");
                context.Writer.WriteLine("///         <see cref=\"ChangeTracker.AutoDetectChangesEnabled\" />.");
                context.Writer.WriteLine("///     </para>");
                context.Writer.WriteLine("/// </summary>");
                context.Writer.WriteLine("/// <returns>");
                context.Writer.WriteLine("///     The number of state entries written to the database.");
                context.Writer.WriteLine("/// </returns>");
                context.Writer.WriteLine("/// <exception cref=\"DbUpdateException\">");
                context.Writer.WriteLine("///     An error is encountered while saving to the database.");
                context.Writer.WriteLine("/// </exception>");
                context.Writer.WriteLine("/// <exception cref=\"DbUpdateConcurrencyException\">");
                context.Writer.WriteLine("///     A concurrency violation is encountered while saving to the database.");
                context.Writer.WriteLine("///     A concurrency violation occurs when an unexpected number of rows are affected during save.");
                context.Writer.WriteLine("///     This is usually because the data in the database has been modified since it was loaded into memory.");
                context.Writer.WriteLine("/// </exception>");
                context.Writer.WriteLine("int SaveChanges();");

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();


            }
        }
    }
}
