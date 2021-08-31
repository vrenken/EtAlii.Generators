namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ParameterStateMachine : ParameterStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        protected override void OnNextState1Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState1Entered(Activate1Trigger trigger) // string title, int count
        {
            throw new NotImplementedException();
        }

        protected override void OnNextState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState2Entered(Activate2Trigger trigger) => Console.WriteLine($"NextState 2 entered from continue trigger: {string0} {int1} {float2}"); //  string string0, int int1, float float2

        protected override void OnNextState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Entered(Continue1Trigger trigger) => LogTransition(typeof(Continue1Trigger));

        protected override void OnNextState3Entered(Activate3Trigger trigger) => Console.WriteLine($"NextState 3 entered from continue trigger: {customer.Id} {project.Id}"); // Customer customer, Project project

        protected override void OnState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(Continue2Trigger trigger) => LogTransition(typeof(Continue2Trigger));

        protected override void OnState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Continue3Trigger trigger) => LogTransition(typeof(Continue3Trigger));
    }
}
