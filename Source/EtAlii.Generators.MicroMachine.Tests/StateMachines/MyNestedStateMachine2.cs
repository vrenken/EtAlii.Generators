namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class MyNestedStateMachine2 : MyNestedStateMachine2Base
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        private void LogTransition(string parameters, Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name}: {parameters})");

        protected override void OnState1Entered(Trigger trigger, State1Choices choices) => LogTransition(typeof(Trigger));
        protected override void OnState1Entered(StartTrigger trigger, State1Choices choices)
        {
            LogTransition($"{trigger.Name}", typeof(StartTrigger));
            choices.Continue();
        }

        protected override void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(Trigger trigger, State2Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState2Entered(ContinueTrigger trigger, State2Choices choices)
        {
            LogTransition(typeof(ContinueTrigger));
            Continue();
        }

        protected override void OnState2Entered(CheckTrigger trigger, State2Choices choices) => LogTransition($"{trigger.Name}", typeof(CheckTrigger));

        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(Trigger trigger, State3Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState3Entered(ContinueTrigger trigger, State3Choices choices) => LogTransition(typeof(ContinueTrigger));

        protected override Task OnState3Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState4Entered(Trigger trigger, State4Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState4Entered(ContinueTrigger trigger, State4Choices choices) => Task.Run(() => LogTransition(typeof(ContinueTrigger)));

        protected override void OnState4Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSubState1Entered(Trigger trigger, SubState1Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnSubState1Entered(StartState2Trigger trigger, SubState1Choices choices) => LogTransition(typeof(StartState2Trigger));

        protected override void OnSubState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnSubState2Entered(Trigger trigger, SubState2Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnSubState2Entered(ContinueTrigger trigger, SubState2Choices choices) => LogTransition(typeof(ContinueTrigger));

        protected override void OnSubState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));
    }
}
