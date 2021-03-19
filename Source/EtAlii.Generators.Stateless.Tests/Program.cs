namespace EtAlii.Generators.Stateless.Tests
{
    public static class Program
    {
        public static void Main()
        {
            var stateMachine = new MyFancyStateMachine();
            stateMachine.Start("My name");
        }
    }
}
