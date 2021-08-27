namespace EtAlii.Generators.MicroMachine.Tests.Nested
{
    public class Customer
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public Customer(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
