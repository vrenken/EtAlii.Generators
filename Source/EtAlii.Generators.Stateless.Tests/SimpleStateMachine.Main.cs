namespace EtAlii.Generators.Stateless.Tests
{
    using System;

    public class Program
    {
        public void Main()
        {
            var stateMachine = new SimpleStateMachine();
            stateMachine.Start();
            Console.WriteLine(stateMachine.Actions[0]);
            stateMachine.Continue();
        }
    }
}
