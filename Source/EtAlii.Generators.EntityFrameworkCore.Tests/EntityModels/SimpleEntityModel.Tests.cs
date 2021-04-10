namespace EtAlii.Generators.EntityFrameworkCore.Tests
{
    using System.Linq;
    using Xunit;

    public class SimpleEntityModelTests
    {
        [Fact]
        public void SimpleEntityModel_CreateDbContext()
        {
            // Arrange.

            // Act.
            using var dataContext = (ISimpleEntityModelDbContext)new SimpleEntityModelDbContext();

            // Assert.
            Assert.NotNull(dataContext);
        }

        [Fact]
        public void SimpleEntityModel_DbContext_SimpleSaveAndLoad()
        {
            // Arrange.
            using var dataContext1 = (ISimpleEntityModelDbContext)new SimpleEntityModelDbContext();

            // Act.
            dataContext1.Users.Add(new User {Email = "john@does.com", Name ="John Doe"});
            dataContext1.SaveChanges();

            // Assert.
            using var dataContext2 = new SimpleEntityModelDbContext();
            var user = dataContext2.Users.Single();
            Assert.Equal("John Doe", user.Name);
            Assert.Equal("john@does.com", user.Email);
        }
    }
}
