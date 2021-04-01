namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Threading.Tasks;

    public class MyNestedStateMachine4 : MyNestedStateMachine4Base
    {
        protected override void OnState1Entered() => Console.WriteLine("State1 entered");
        protected override void OnState1EnteredFromStartTrigger(string name)
        {
            Console.WriteLine($"Name: {name}");
            Continue();
        }

        protected override void OnState1Exited() => Console.WriteLine("State1 exited");

        protected override Task OnState2EnteredAsync()
        {
            Console.WriteLine("State2 entered");
            return Task.CompletedTask;
        }

        protected override void OnState2Exited() => Console.WriteLine("State2 exited");

        protected override void OnState3Entered() => Console.WriteLine("State3 entered");

        protected override Task OnState3ExitedAsync()
        {
            Console.WriteLine("State3 exited");
            return Task.CompletedTask;
        }

        protected override Task OnState4EnteredAsync()
        {
            Console.WriteLine("State4 entered");
            return Task.CompletedTask;
        }

        protected override Task OnState4ExitedAsync()
        {
            Console.WriteLine("State4 exited");
            return Task.CompletedTask;
        }
    }
}