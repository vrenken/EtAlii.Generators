namespace EtAlii.Generators.Stateless.Tests
{
    public static class Program
    {
        public static void Main()
        {
            var stateMachine = new SimpleStateMachine();
            stateMachine.Start();
            stateMachine.Continue();
        }
    }
}
