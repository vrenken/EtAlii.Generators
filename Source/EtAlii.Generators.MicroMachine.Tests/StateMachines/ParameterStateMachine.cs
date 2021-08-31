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

        protected override void OnNextState1Entered(Activate1Trigger trigger) => Transitions.Add($"OnNextState1Entered(Activate1Trigger1: {trigger.Title} {trigger.Count})");

        protected override void OnNextState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState2Entered(Activate2Trigger trigger) => Transitions.Add($"OnNextState2Entered(Activate2Trigger1: {trigger.String0} {trigger.Int1} {trigger.Float2})");

        protected override void OnNextState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Entered(Continue1Trigger trigger) => LogTransition(typeof(Continue1Trigger));

        protected override void OnNextState3Entered(Activate3Trigger trigger) => Console.WriteLine($"OnNextState3Entered(Activate3Trigger1: {trigger.Customer.Id} {trigger.Project.Id})");

        protected override void OnState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(Continue2Trigger trigger) => LogTransition(typeof(Continue2Trigger));

        protected override void OnState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Continue3Trigger trigger) => LogTransition(typeof(Continue3Trigger));
    }
}
