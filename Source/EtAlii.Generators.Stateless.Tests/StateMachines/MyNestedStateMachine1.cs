namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Threading.Tasks;

    public class MyNestedStateMachine1 : MyNestedStateMachine1Base
    {
        protected override void OnState1Entered() => Console.WriteLine("State1 entered");
        protected override void OnState1EnteredFromStartTrigger(string name)
        {
            Console.WriteLine($"Name: {name}");
            Continue();
        }

        protected override void OnState1Exited() => Console.WriteLine("State1 exited");

        protected override void OnState2Entered() => Console.WriteLine("State2 entered");

        protected override void OnSubState2EnteredFromContinueTrigger()
        {
            Console.WriteLine("Inside State2");
            Continue();
        }

        protected override void OnState2Exited() => Console.WriteLine("State2 exited");

        protected override void OnState3Entered() => Console.WriteLine("State3 entered");

        protected override Task OnState3ExitedAsync() => Task.Run(() => Console.WriteLine("State3 exited"));

        protected override Task OnState4EnteredAsync()
        {
            Console.WriteLine("State4 entered");
            return Task.CompletedTask;
        }

        protected override void OnState4Exited() => Console.WriteLine("State4 exited");
    }
}
