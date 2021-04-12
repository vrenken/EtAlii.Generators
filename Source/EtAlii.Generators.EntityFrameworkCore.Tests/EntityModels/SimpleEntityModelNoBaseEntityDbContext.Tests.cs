// ReSharper disable once CheckNamespace
namespace EtAlii.Generators.EntityFrameworkCore.NoBaseEntity.Tests
{
    using System.Linq;
    using Xunit;

    public class SimpleEntityModelNoBaseEntityDbContextTests
    {
        [Fact]
        public void SimpleEntityModelNoBaseEntityDbContext_CreateDbContext()
        {
            // Arrange.

            // Act.
            using var dataContext = (ISimpleEntityModelNoBaseEntityDbContext)new SimpleEntityModelNoBaseEntityDbContext();

            // Assert.
            Assert.NotNull(dataContext);
        }

        [Fact]
        public void SimpleEntityModelNoBaseEntityDbContext_DbContext_SimpleSaveAndLoad()
        {
            // Arrange.
            using var dataContext1 = (ISimpleEntityModelNoBaseEntityDbContext)new SimpleEntityModelNoBaseEntityDbContext();

            // Act.
            dataContext1.Users.Add(new User {Email = "john@does.com", Name ="John Doe"});
            dataContext1.SaveChanges();

            // Assert.
            using var dataContext2 = new SimpleEntityModelNoBaseEntityDbContext();
            var user = dataContext2.Users.Single();
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("john@does.com", user.Email);
        }
    }
}
