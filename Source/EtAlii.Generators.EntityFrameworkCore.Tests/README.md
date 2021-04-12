# PlantUML diagram to Entity Framework Core entities and DbContext code

## Features.
- On the fly (and in-IDE) code generation for EF Core using in-solution PlantUML class diagrams.
- Generates entities for all defined classes.
- Generates a base DbContext in which all class properties and relations are configured.

## How to start.
Usage:

1. Add the analyzer NuGet package to the target project:
   ```csproj
    <ItemGroup>
        <PackageReference Update="EtAlii.Generators.EntityFrameworkCore" Version="1.0.1" PrivateAssets="all" />
    </ItemGroup>
   ```

2. Also add the corresponding EntityFrameworkCore NuGet packages to the target project:
   ```csproj
   <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="5.0.5" />
   </ItemGroup>
   ```
   Make sure to replace the InMemory package with another one that matches the database that needs to be accessed.

3. Come up with a fancy PlantUML class diagram, for example:

   ![EntityModels/SimpleEntityModel.puml](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/vrenken/EtAlii.Generators/main/Source/EtAlii.Generators.EntityFrameworkCore.Tests/EntityModels/SimpleEntityModel.puml)

4. Put the PlantUML diagram in a file, for example `SimpleEntityModel.puml` as specified below:
   ```puml
    @startuml

    'efcore namespace EtAlii.Generators.EntityFrameworkCore.Tests
    'efcore using EtAlii.Generators.EntityFrameworkCore.Tests.Nested
    'efcore entity EntityBase
    'efcore dbcontext SimpleEntityModelDbContextBase
    'efcore interface ISimpleEntityModelDbContext

    'efcore map 'From' 'Sent'
    Message "1" --* "0..n" User

    'efcore map 'To' 'Received'
    Message "1" --* "0..n" User

    'efcore map 'Tweets' 'User'
    User "0..n" *-- "1" Tweet

    'efcore map 'Tweet' 'Image'
    Image "1" -- "0..1" Tweet

    'efcore map 'Users'
    class User
    {
    +Name: string
    -Email: string
    Sent: Message[]
    Received: Message[]
    Tweets: Tweet[]
    }

    'efcore map 'Messages'
    class Message {
    +Text: string
    From: User
    To: User
    }

    'efcore map 'Tweets'
    class Tweet{
    +Text: string
    +User: User
    Image: Image
    }

    'efcore map 'Images'
    class Image{
    +Data: byte[]
    Tweet: Tweet
    }

    @enduml
   ```
   Make sure to notice the ``efcore`` parameters added at the start of the file. They help the code generation process understand which class/namespace to generate.<br/>
   Also notice the mappings on top of the classes and relations. They also assist with the naming of the generated code.

5. Reference the file from the project (.csproj) as a `EfCoreModel` entry:
   ```csproj
     <ItemGroup>
       <EfCoreModel Include="SimpleEntityModel.puml" />
     </ItemGroup>
   ```
6. Compile the project - Antlr is used to parse the puml file and instruct the Roslyn generator to create C# EF Core code entities and DbContext configurations according to the classes and relations defined in the diagram.


7. Add a class file to implement the base DbContext implementation. Override the OnConfiguring to specify which type of database to use.
   ```cs
    namespace EtAlii.Generators.EntityFrameworkCore.Tests
    {
        using Microsoft.EntityFrameworkCore;

        public class SimpleEntityModelDbContext : SimpleEntityModelDbContextBase
        {
            protected override void OnConfiguring(DbContextOptionsBuilder options)
            {
                options.UseInMemoryDatabase("Test database 1");

                base.OnConfiguring(options);
            }
        }
    }
   ```
8. Execute database queries from somewhere in your code, for example as done in the unit test method below.
   ```cs
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
   ```
9. Enjoy the magic that code-generation can bring to your project.


10. Star this project if you like it :-)


