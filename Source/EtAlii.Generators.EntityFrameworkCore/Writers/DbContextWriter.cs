namespace EtAlii.Generators.EntityFrameworkCore
{
    using System;
    using System.Linq;

    public class DbContextWriter
    {
        public void Write(WriteContext<EntityModel> context)
        {
            var hasDbContextName = !string.IsNullOrWhiteSpace(context.Instance.DbContextName);
            if (hasDbContextName)
            {
                var hasInterface = !string.IsNullOrEmpty(context.Instance.InterfaceName);

                var postfix = hasInterface
                    ? $", {context.Instance.InterfaceName}"
                    : "";
                context.Writer.WriteLine($"public class {context.Instance.DbContextName} : global::Microsoft.EntityFrameworkCore.DbContext {postfix}");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;

                foreach (var @class in context.Instance.Classes)
                {
                    var propertyName = @class.Mapping?.Name ?? @class.Name;
                    context.Writer.WriteLine($"public virtual DbSet<{@class.Name}> {propertyName} {{ get; set; }}");
                }

                context.Writer.WriteLine();
                context.Writer.WriteLine("protected override void OnModelCreating(ModelBuilder modelBuilder)");
                context.Writer.WriteLine("{");
                context.Writer.Indent += 1;

                foreach (var @class in context.Instance.Classes)
                {
                    var propertyName = @class.Mapping?.Name ?? @class.Name;
                    context.Writer.WriteLine($"OnConfigure{propertyName}(modelBuilder.Entity<{@class.Name}>());");
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();

                foreach (var @class in context.Instance.Classes)
                {
                    var propertyName = @class.Mapping?.Name ?? @class.Name;

                    context.Writer.WriteLine($"protected virtual void OnConfigure{propertyName}(EntityTypeBuilder<{@class.Name}> builder)");
                    context.Writer.WriteLine("{");
                    context.Writer.Indent += 1;

                    WriteComment(context, @class.Source.Text);
                    context.Writer.WriteLine();

                    var hasBaseEntity = !string.IsNullOrWhiteSpace(context.Instance.EntityName);
                    if (hasBaseEntity)
                    {
                        context.Writer.WriteLine("// Base entity index.");
                        context.Writer.WriteLine("builder");
                        context.Writer.WriteLine("\t.HasIndex(entity => entity.Id)");
                        context.Writer.WriteLine("\t.IsUnique();");
                        context.Writer.WriteLine("builder");
                        context.Writer.WriteLine("\t.HasKey(entity => entity.Id);");
                        context.Writer.WriteLine();
                    }

                    foreach (var property in @class.Properties)
                    {
                        var fromRelation = context.Instance.Relations
                            .Where(r => r.From == @class.Name)
                            .SingleOrDefault(r => r.Mapping.FromProperty == property.Name);
                        var toRelation = context.Instance.Relations
                            .Where(r => r.To == @class.Name)
                            .SingleOrDefault(r => r.Mapping.ToProperty == property.Name);
                        if (fromRelation != null)
                        {
                            var fromProperty = fromRelation.Mapping.FromProperty;
                            var fromCardinality = fromRelation.FromCardinality;
                            var toProperty = fromRelation.Mapping.ToProperty;
                            var toCardinality = fromRelation.ToCardinality;
                            var toClass = fromRelation.To;
                            WriteComment(context, fromRelation.Source.Text);
                            WriteRelationProperty(context, fromProperty, fromCardinality, toProperty, toCardinality, toClass);
                        }
                        else if (toRelation != null)
                        {
                            // We need to reverse the properties and their cardinality.
                            var fromProperty = toRelation.Mapping.ToProperty;
                            var fromCardinality = toRelation.ToCardinality;
                            var toProperty = toRelation.Mapping.FromProperty;
                            var toCardinality = toRelation.FromCardinality;
                            var toClass = toRelation.From;
                            WriteComment(context, toRelation.Source.Text);
                            WriteRelationProperty(context, fromProperty, fromCardinality, toProperty, toCardinality, toClass);
                        }
                        else
                        {
                            WriteComment(context, property.Source.Text);
                            WriteSimpleProperty(context, property);
                        }
                    }

                    context.Writer.Indent -= 1;
                    context.Writer.WriteLine("}");
                    context.Writer.WriteLine();
                }

                context.Writer.Indent -= 1;
                context.Writer.WriteLine("}");
                context.Writer.WriteLine();

                WriteInterface(context);
            }
        }

        private void WriteComment(WriteContext<EntityModel> context, string text)
        {
            var lines = text.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToArray();
            foreach (var line in lines)
            {
                context.Writer.WriteLine($"// {line.Trim()}");
            }
        }

        private void WriteRelationProperty(
            WriteContext<EntityModel> context,
            string fromProperty, Cardinality fromCardinality,
            string toProperty, Cardinality toCardinality,
            string toClass)
        {
            context.Writer.WriteLine("builder");

            switch (fromCardinality)
            {
                case Cardinality.NoneOrOne:
                case Cardinality.One:
                    context.Writer.WriteLine($"\t.HasOne(entity => entity.{fromProperty})");
                    break;
                case Cardinality.OneOrMore:
                case Cardinality.NoneOrMore:
                    context.Writer.WriteLine($"\t.HasMany(entity => entity.{fromProperty})");
                    break;
            }
            switch (toCardinality)
            {
                case Cardinality.NoneOrOne:
                case Cardinality.One:
                    context.Writer.WriteLine($"\t.WithOne(entity => entity.{toProperty})");
                    break;
                case Cardinality.OneOrMore:
                case Cardinality.NoneOrMore:
                    context.Writer.WriteLine($"\t.WithMany(entity => entity.{toProperty})");
                    break;
            }

            if (fromCardinality == Cardinality.NoneOrOne && toCardinality == Cardinality.One)
            {
                context.Writer.WriteLine($"\t.HasForeignKey(\"{toClass}\")");
            }
            else if (fromCardinality == Cardinality.One && toCardinality == Cardinality.One)
            {
                context.Writer.WriteLine($"\t.HasForeignKey(\"{toClass}\")");
            }

            switch (fromCardinality)
            {
                case Cardinality.One:
                case Cardinality.OneOrMore:
                    context.Writer.WriteLine("\t.IsRequired(true);");
                    break;
                case Cardinality.NoneOrOne:
                case Cardinality.NoneOrMore:
                    context.Writer.WriteLine("\t.IsRequired(false);");
                    break;
            }
            context.Writer.WriteLine();
        }

        private void WriteSimpleProperty(WriteContext<EntityModel> context, Property property)
        {
            context.Writer.WriteLine($"builder.Property(entity => entity.{property.Name}).IsRequired();");
            context.Writer.WriteLine();
        }

        private static void WriteInterface(WriteContext<EntityModel> context)
        {
            var hasInterface = !string.IsNullOrEmpty(context.Instance.InterfaceName);
            if (hasInterface)
            {
                context.Writer.WriteLine($"public interface {context.Instance.InterfaceName} : global::System.IDisposable");
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
            }
        }
    }
}
