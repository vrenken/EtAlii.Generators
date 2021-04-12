// ReSharper disable once CheckNamespace
namespace EtAlii.Generators.EntityFrameworkCore.NoBaseEntity.Tests
{
    using Microsoft.EntityFrameworkCore;

    public class SimpleEntityModelNoBaseEntityDbContext : SimpleEntityModelNoBaseEntityDbContextBase
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("Test database 2");

            base.OnConfiguring(options);
        }
    }
}
