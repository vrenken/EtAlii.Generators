namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Threading.Tasks;

    public class MyNestedStateMachine1 : MyNestedStateMachine1Base
    {
        protected override void OnState1Entered(State1EventArgs e) => Console.WriteLine("State 1 entered");
        protected override void OnState1EnteredFromStartTrigger(State1EventArgs e, string name)
        {
            Console.WriteLine($"Name: {name}");
            Continue();
        }

        protected override void OnState1Exited() => Console.WriteLine("State 1 exited");

        protected override void OnState2Entered(State2EventArgs e) => Console.WriteLine("State 2 entered");

        protected override void OnSubState2EnteredFromContinueTrigger(SubState2EventArgs e)
        {
            Console.WriteLine("Inside State 2");
            Continue();
        }

        protected override void OnState2Exited() => Console.WriteLine("State 2 exited");

        protected override void OnState3Entered(State3EventArgs e) => Console.WriteLine("State 3 entered");

        protected override Task OnState3Exited() => Task.Run(() => Console.WriteLine("State 3 exited"));

        protected override Task OnState4Entered(State4EventArgs e)
        {
            Console.WriteLine("State 4 entered");
            return Task.CompletedTask;
        }

        protected override void OnState4Exited() => Console.WriteLine("State 4 exited");

        protected override void OnState2InternalCheckTrigger(string name) => Console.WriteLine($"State 2 internal Check triggered: {name}");

        protected override void OnState3EnteredFromContinueTrigger(State3EventArgs e) => Console.WriteLine("State 3 entered from continue trigger");

        protected override Task OnState4EnteredFromContinueTrigger(State4EventArgs e) => Task.Run(() => Console.WriteLine("State 4 entered from continue trigger"));

        protected override void OnSubState1Entered(SubState1EventArgs e) => Console.WriteLine("SubState 1 entered");

        protected override void OnSubState1Exited() => Console.WriteLine("SubState 1 exited");

        protected override void OnSubState2Entered(SubState2EventArgs e) => Console.WriteLine("SubState 2 entered");

        protected override void OnSubState2Exited() => Console.WriteLine("SubState 2 exited");
    }
}
