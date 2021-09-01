namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;

    public class Program
    {
        public void Main()
        {
            var stateMachine = new SimpleStateMachine();
            stateMachine.Start();
            Console.WriteLine(stateMachine.Transitions[0]);
            stateMachine.Continue();
        }
    }
}
