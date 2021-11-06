namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ChoiceStateMachine : ChoiceStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");


        protected override void OnState1Entered(Trigger trigger, State1Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(Trigger trigger, State2Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Trigger trigger, State3Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(UpTrigger trigger, State3Choices choices) => LogTransition(typeof(UpTrigger));

        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState4Entered(Trigger trigger, State4Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnState4Entered(DownTrigger trigger, State4Choices choices) => LogTransition(typeof(DownTrigger));

        protected override void OnState1Entered(StartTrigger trigger, State1Choices choices) => LogTransition(typeof(StartTrigger));

        protected override void OnState2Entered(ContinueTrigger trigger, State2Choices choices) => LogTransition(typeof(ContinueTrigger));

        protected override void OnState4Exited(Trigger trigger) => LogTransition(typeof(Trigger));
    }
}
