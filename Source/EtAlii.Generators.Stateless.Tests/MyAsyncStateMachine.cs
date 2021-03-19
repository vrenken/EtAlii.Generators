namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Threading.Tasks;

    public partial class MyAsyncStateMachine : MyAsyncStateMachineBase
    {
        protected override void OnState1Entered() => Console.WriteLine("State1 entered");
        protected override async Task OnState1EnteredFromStartTrigger(string name)
        {
            Console.WriteLine($"Name: {name}");
            await ContinueAsync().ConfigureAwait(false);
        }

        protected override void OnState1Exited() => Console.WriteLine("State1 exited");

        protected override void OnState2Entered() => Console.WriteLine("State2 entered");

        protected override Task OnState2EnteredFromContinueTrigger()
        {
            Console.WriteLine("Inside State2");
            Continue();
            return Task.CompletedTask;
        }

        protected override void OnState2Exited() => Console.WriteLine("State2 exited");

        protected override void OnState3Entered() => Console.WriteLine("State3 entered");
        protected override void OnState3Exited() => Console.WriteLine("State3 exited");

        protected override void OnState4Entered() => Console.WriteLine("State4 entered");
        protected override void OnState4Exited() => Console.WriteLine("State4 exited");
    }
}
