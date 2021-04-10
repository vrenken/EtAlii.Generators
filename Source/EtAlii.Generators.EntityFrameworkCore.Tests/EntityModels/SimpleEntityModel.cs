namespace EtAlii.Generators.EntityFrameworkCore.Tests
{
    using Microsoft.EntityFrameworkCore;

    public class SimpleEntityModelDbContext : SimpleEntityModelDbContextBase
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseInMemoryDatabase("Test database");

            base.OnConfiguring(options);
        }
    }
}
