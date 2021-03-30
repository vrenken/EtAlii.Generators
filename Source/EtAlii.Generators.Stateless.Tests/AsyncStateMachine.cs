namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Threading.Tasks;

    public class AsyncStateMachine : AsyncStateMachineBase
    {
        protected override Task OnState1EnteredAsync()
        {
            Console.WriteLine("State1 entered");
            return Task.CompletedTask;
        }

        protected override Task OnState1ExitedAsync()
        {
            Console.WriteLine("State1 exited");
            return Task.CompletedTask;
        }

        protected override Task OnState2EnteredAsync()
        {
            Console.WriteLine("State2 entered");
            return Task.CompletedTask;
        }

        protected override Task OnState2EnteredFromContinueTrigger()
        {
            Console.WriteLine("Inside State2");
            Continue();
            return Task.CompletedTask;
        }

        protected override void OnState2Exited() => Console.WriteLine("State2 exited");

        protected override Task OnState3EnteredAsync()
        {
            Console.WriteLine("State3 entered");
            return Task.CompletedTask;
        }

        protected override void OnState3Exited() => Console.WriteLine("State3 exited");

        protected override void OnState4Entered() => Console.WriteLine("State4 entered");

        protected override Task OnState4ExitedAsync()
        {
            Console.WriteLine("State4 exited");
            return Task.CompletedTask;
        }
    }
}
