namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using EtAlii.Generators.Stateless.Tests.Nested;

    public class ParameterStateMachine : ParameterStateMachineBase
    {
        protected override void OnNextState1Entered() => Console.WriteLine("NextState 1 entered");

        protected override void OnNextState1Exited() => Console.WriteLine("NextState 1 exited");

        protected override void OnNextState1EnteredFromActivate1Trigger(string title, int count)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextState2Entered() => Console.WriteLine("NextState 2 entered");

        protected override void OnNextState2Exited() => Console.WriteLine("NextState 2 exited");

        protected override void OnNextState2EnteredFromContinueTrigger(string string0, int int1, float float2) => Console.WriteLine($"NextState 2 entered from continue trigger: {string0} {int1} {float2}");

        protected override void OnNextState3Entered() => Console.WriteLine("NextState 3 entered");

        protected override void OnNextState3Exited() => Console.WriteLine("NextState 3 exited");

        protected override void OnState1Entered() => Console.WriteLine("State 1 entered");

        protected override void OnState1Exited() => Console.WriteLine("State 1 exited");

        protected override void OnState1EnteredFromContinue1Trigger() => Console.WriteLine("State 1 entered from continue 1 trigger");

        protected override void OnNextState3EnteredFromContinueTrigger(Customer customer, Project project) => Console.WriteLine($"NextState 3 entered from continue trigger: {customer.Id} {project.Id}");

        protected override void OnState2Entered() => Console.WriteLine("State 2 entered");

        protected override void OnState2Exited() => Console.WriteLine("State 2 exited");

        protected override void OnState2EnteredFromContinue2Trigger() => Console.WriteLine("State 2 entered from continue 2 trigger");

        protected override void OnState3Entered() => Console.WriteLine("State 3 entered");

        protected override void OnState3Exited() => Console.WriteLine("State 3 exited");

        protected override void OnState3EnteredFromContinue3Trigger() => Console.WriteLine("State 3 entered from continue 3 trigger");
    }
}
