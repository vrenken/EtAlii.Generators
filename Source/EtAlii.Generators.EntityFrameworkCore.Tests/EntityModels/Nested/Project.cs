namespace EtAlii.Generators.EntityFrameworkCore.Tests.Nested
{
    public class Project
    {
        public int Id { get; }
        public string Name { get; }

        public Project(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
