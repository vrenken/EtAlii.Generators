namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Threading.Tasks;

    public class MyNestedStateMachine4 : MyNestedStateMachine4Base
    {
        protected override void OnState1Entered() => Console.WriteLine("State 1 entered");
        protected override void OnState1EnteredFromStartTrigger(string name)
        {
            Console.WriteLine($"Name: {name}");
            Continue();
        }

        protected override void OnState1Exited() => Console.WriteLine("State 1 exited");

        protected override void OnState2Entered() => Console.WriteLine("State 2 entered");

        protected override void OnState2Exited() => Console.WriteLine("State 2 exited");

        protected override void OnState3Entered() => Console.WriteLine("State 3 entered");

        protected override Task OnState3ExitedAsync() => Task.Run(() => Console.WriteLine("State 3 exited"));

        protected override Task OnState4EnteredAsync() => Task.Run(() => Console.WriteLine("State 4 entered"));

        protected override void OnState4Exited() => Console.WriteLine("State 4 exited");

        protected override void OnState2InternalCheckTrigger(string name) => Console.WriteLine($"State 2 internal check trigger: {name}");

        protected override void OnState3EnteredFromContinueTrigger() => Console.WriteLine($"State 2 entered from continue trigger");

        protected override Task OnState4EnteredFromContinueTrigger() => Task.Run(() => Console.WriteLine($"State 4 entered from continue trigger"));

        protected override void OnSubState1Entered() => Console.WriteLine("SubState 1 entered");

        protected override void OnSubState1Exited() => Console.WriteLine("SubState 1 exited");

        protected override void OnSubState1EnteredFromContinueTrigger() => Console.WriteLine("SubState 1 entered from continue trigger");

        protected override void OnSubState2Entered() => Console.WriteLine("SubState 2 entered");

        protected override void OnSubState2Exited() => Console.WriteLine("SubState 2 exited");

        protected override void OnSubState2EnteredFromContinueTrigger() => Console.WriteLine("SubState 2 entered from continue trigger");
    }
}
