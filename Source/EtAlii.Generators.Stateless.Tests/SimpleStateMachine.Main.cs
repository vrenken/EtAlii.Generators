namespace EtAlii.Generators.Stateless.Tests
{
    using System;

    public static class Program
    {
        public static void Main()
        {
            var stateMachine = new SimpleStateMachine();
            stateMachine.Start();
            Console.WriteLine(stateMachine.Actions[0]);
            stateMachine.Continue();
        }
    }
}
