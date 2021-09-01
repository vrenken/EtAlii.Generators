namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SimpleStateMachine : SimpleStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        protected override void OnState1Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState1Entered(StartTrigger trigger) => LogTransition(typeof(StartTrigger));
        protected override void OnState1Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState2Entered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        protected override void OnState2Entered(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));
        protected override void OnState2Exited(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));
        protected override void OnState2Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState3Entered(ContinueTrigger trigger)
        {
            LogTransition(typeof(ContinueTrigger));
            Continue();
        }
        protected override void OnState3Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState4Entered(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState4Entered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        protected override void OnState4Exited(Trigger trigger) => LogTransition(typeof(Trigger));

    }
}
