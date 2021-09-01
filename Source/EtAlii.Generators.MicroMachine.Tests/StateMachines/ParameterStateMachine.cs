namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    public class ParameterStateMachine : ParameterStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        private void LogTransition(string parameters, Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name}: {parameters})");


        protected override void OnState1Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState1Entered(Continue1Trigger trigger) => LogTransition(typeof(Continue1Trigger));
        protected override void OnState1Exited(Activate1Trigger trigger) => LogTransition($"{trigger.Title}, {trigger.Count}", typeof(Activate1Trigger));
        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState1Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnNextState1Entered(Activate1Trigger trigger) => LogTransition($"{trigger.Title}, {trigger.Count}", typeof(Activate1Trigger));
        protected override void OnNextState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState2Entered(Continue2Trigger trigger) => LogTransition(typeof(Continue2Trigger));
        protected override void OnState2Exited(Activate2Trigger trigger) => LogTransition($"{trigger.String0}, {trigger.Int1}, {trigger.Float2.ToString(CultureInfo.InvariantCulture)}", typeof(Activate2Trigger));
        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnNextState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnNextState2Entered(Activate2Trigger trigger) => LogTransition($"{trigger.String0}, {trigger.Int1}, {trigger.Float2.ToString(CultureInfo.InvariantCulture)}", typeof(Activate2Trigger));
        protected override void OnNextState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState3Entered(Continue3Trigger trigger) => LogTransition(typeof(Continue3Trigger));
        protected override void OnState3Exited(Activate3Trigger trigger) => LogTransition($"{trigger.Customer.Id}, {trigger.Project.Id}", typeof(Activate3Trigger));
        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));


        protected override void OnNextState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnNextState3Entered(Activate3Trigger trigger) => LogTransition($"{trigger.Customer.Id}, {trigger.Project.Id}", typeof(Activate3Trigger));
        protected override void OnNextState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

    }
}
