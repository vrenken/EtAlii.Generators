namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class MyNestedStateMachine1 : MyNestedStateMachine1Base
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        protected override void OnState1Entered(Trigger trigger, State1Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState1Entered(StartTrigger trigger, State1Choices choices) => LogTransition(typeof(StartTrigger));

        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState1Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        protected override void OnState2Entered(Trigger trigger, State2Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(CheckTrigger trigger, State2Choices choices) => LogTransition(typeof(CheckTrigger));

        protected override void OnState2Entered(ContinueTrigger trigger, State2Choices choices) => LogTransition(typeof(ContinueTrigger));

        protected override void OnSubState1Entered(Trigger trigger, SubState1Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnSubState1Entered(ContinueTrigger trigger, SubState1Choices choices) => LogTransition(typeof(ContinueTrigger));

        protected override void OnSubState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSubState1Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        protected override void OnSubState1Exited(ExitTrigger trigger) => LogTransition(typeof(ExitTrigger));

        protected override void OnSubState1Exited(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));

        protected override void OnSubState2Entered(Trigger trigger, SubState2Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnSubState2Entered(ContinueTrigger trigger, SubState2Choices choices) => LogTransition(typeof(ContinueTrigger));

        protected override void OnSubState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSubState2Exited(ExitTrigger trigger) => LogTransition(typeof(ExitTrigger));

        protected override void OnSubState2Exited(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));

        protected override void OnState2Exited(ExitTrigger trigger) => LogTransition(typeof(ExitTrigger));

        protected override void OnState2Exited(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));

        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Trigger trigger, State3Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(ExitTrigger trigger, State3Choices choices) => LogTransition(typeof(ExitTrigger));

    }
}
