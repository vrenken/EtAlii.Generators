namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SubStateStateMachine : SubStateStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        protected override void OnState1Entered(Trigger trigger, State1Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnState1Entered(Continue1Trigger trigger, State1Choices choices) => LogTransition(typeof(Continue1Trigger));
        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState1Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));


        protected override void OnState2Entered(Trigger trigger, State2Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnState2Entered(Continue2Trigger trigger, State2Choices choices) => LogTransition(typeof(Continue2Trigger));
        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState2Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));


        protected override void OnState3Entered(Trigger trigger, State3Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnState3Entered(Continue3Trigger trigger, State3Choices choices) => LogTransition(typeof(Continue3Trigger));
        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));
        protected override void OnState3Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));


        protected override void OnSuperState1Entered(Trigger trigger, SuperState1Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnSuperState1Entered(ContinueTrigger trigger, SuperState1Choices choices) => LogTransition(typeof(ContinueTrigger));
        protected override void OnSuperState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));


        protected override void OnSuperState2Entered(Trigger trigger, SuperState2Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnSuperState2Entered(ContinueTrigger trigger, SuperState2Choices choices) => LogTransition(typeof(ContinueTrigger));
        protected override void OnSuperState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSuperState3Entered(Trigger trigger, SuperState3Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnSuperState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));
        // TODO
        //protected override void OnSuperState3Entered(ContinueTrigger trigger) => LogTrigger(typeof(ContinueTrigger));

        protected override void OnSubState1Entered(Trigger trigger, SubState1Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnSubState1Entered(_IdleToSubState1Trigger trigger, SubState1Choices choices) => LogTransition(typeof(_IdleToSubState1Trigger));
        protected override void OnSubState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSubState2Entered(Trigger trigger, SubState2Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnSubState2Entered(StartTrigger trigger, SubState2Choices choices) => LogTransition(typeof(StartTrigger));
        protected override void OnSubState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSubState3Entered(Trigger trigger, SubState3Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnSubState3Entered(ContinueTrigger trigger, SubState3Choices choices) => LogTransition(typeof(ContinueTrigger));
        protected override void OnSubState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

    }
}
