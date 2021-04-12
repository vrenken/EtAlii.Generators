namespace EtAlii.Generators.EntityFrameworkCore.Tests
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public void SimpleEntityModel_DbContext_NestedSaveAndLoad()
        {
            // Arrange.
            using var dataContext1 = (ISimpleEntityModelDbContext)new SimpleEntityModelDbContext();
            var user = new User {Email = "john@does.com", Name = "John Doe"};
            var tweet = new Tweet {Text = "Test tweet", User = user};

            // Act.
            dataContext1.Entry(tweet).State = EntityState.Added;
            dataContext1.Entry(user).State = EntityState.Added;
            dataContext1.SaveChanges();

            // Assert.
            using var dataContext2 = new SimpleEntityModelDbContext();
            var result = dataContext2.Tweets
                .Include(t => t.User)
                .Single();
            Assert.Equal("Test tweet", result.Text);
            Assert.NotNull(result.User);
            Assert.Equal("John Doe", result.User.Name);
            Assert.Equal("john@does.com", result.User.Email);
        }
    }
}
