namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class AsyncSubStateStateMachine : AsyncSubStateStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        protected override Task OnState1Entered(Trigger trigger, State1Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState1Entered(Continue1Trigger trigger, State1Choices choices) => Task.Run(() => LogTransition(typeof(Continue1Trigger)));

        protected override Task OnState1Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState2Entered(Trigger trigger, State2Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState2Entered(Continue2Trigger trigger, State2Choices choices) => Task.Run(() => LogTransition(typeof(Continue2Trigger)));

        protected override Task OnState2Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState3Entered(Trigger trigger, State3Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState3Entered(Continue3Trigger trigger, State3Choices choices) => Task.Run(() => LogTransition(typeof(Continue3Trigger)));

        protected override Task OnState3Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSuperState1Entered(Trigger trigger, SuperState1Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSuperState1Entered(ContinueTrigger trigger, SuperState1Choices choices) => Task.Run(() => LogTransition(typeof(ContinueTrigger)));

        protected override Task OnSuperState1Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSuperState2Entered(Trigger trigger, SuperState2Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSuperState2Entered(ContinueTrigger trigger, SuperState2Choices choices) => Task.Run(() => LogTransition(typeof(ContinueTrigger)));

        protected override Task OnSuperState2Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSubState1Entered(Trigger trigger, SubState1Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSubState2Entered(StartTrigger trigger, SubState2Choices choices) => Task.Run(() => LogTransition(typeof(StartTrigger)));

        protected override Task OnSubState2Entered(Trigger trigger, SubState2Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSubState3Entered(Trigger trigger, SubState3Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSubState3Entered(ContinueTrigger trigger, SubState3Choices choices) => Task.Run(() => LogTransition(typeof(ContinueTrigger)));

        protected override Task OnSuperState3Entered(Trigger trigger, SuperState3Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override void OnSuperState3Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override void OnSubState1Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnSubState1Entered(_IdleToSubState1Trigger trigger, SubState1Choices choices) => Task.Run(() => LogTransition(typeof(_IdleToSubState1Trigger)));

        protected override void OnSubState2Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override void OnSubState3Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));
    }
}
