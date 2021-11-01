namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using EtAlii.Generators.Stateless.Tests.Nested;

    public class ParameterStateMachine : ParameterStateMachineBase
    {
        protected override void OnNextState1Entered(NextState1EventArgs e) => Console.WriteLine("NextState 1 entered");

        protected override void OnNextState1Exited() => Console.WriteLine("NextState 1 exited");

        protected override void OnNextState1EnteredFromActivate1Trigger(NextState1EventArgs e, string title, int count)
        {
            throw new NotImplementedException();
        }

        protected override void OnNextState2Entered(NextState2EventArgs e) => Console.WriteLine("NextState 2 entered");

        protected override void OnNextState2Exited() => Console.WriteLine("NextState 2 exited");

        protected override void OnNextState2EnteredFromContinueTrigger(NextState2EventArgs e, string string0, int int1, float float2) => Console.WriteLine($"NextState 2 entered from continue trigger: {string0} {int1} {float2}");

        protected override void OnNextState3Entered(NextState3EventArgs e) => Console.WriteLine("NextState 3 entered");

        protected override void OnNextState3Exited() => Console.WriteLine("NextState 3 exited");

        protected override void OnState1Entered(State1EventArgs e) => Console.WriteLine("State 1 entered");

        protected override void OnState1Exited() => Console.WriteLine("State 1 exited");

        protected override void OnState1EnteredFromContinue1Trigger(State1EventArgs e) => Console.WriteLine("State 1 entered from continue 1 trigger");

        protected override void OnNextState3EnteredFromContinueTrigger(NextState3EventArgs e, Customer customer, Project project) => Console.WriteLine($"NextState 3 entered from continue trigger: {customer.Id} {project.Id}");

        protected override void OnState2Entered(State2EventArgs e) => Console.WriteLine("State 2 entered");

        protected override void OnState2Exited() => Console.WriteLine("State 2 exited");

        protected override void OnState2EnteredFromContinue2Trigger(State2EventArgs e) => Console.WriteLine("State 2 entered from continue 2 trigger");

        protected override void OnState3Entered(State3EventArgs e) => Console.WriteLine("State 3 entered");

        protected override void OnState3Exited() => Console.WriteLine("State 3 exited");

        protected override void OnState3EnteredFromContinue3Trigger(State3EventArgs e) => Console.WriteLine("State 3 entered from continue 3 trigger");
    }
}
