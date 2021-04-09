namespace EtAlii.Generators.EntityFrameworkCore.Tests
{
    using System;

    public abstract class EntityBase
    {
        /// <summary>
        /// This is the identifier that we use to identify each Entity in the EF Core code/datastore with.
        /// </summary>
        /// <remarks>
        /// The property setter is private as id management is done by the database.</remarks>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Guid Id { get; private set; }
    }
}
