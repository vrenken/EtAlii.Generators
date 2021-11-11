namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public class AsyncStateMachine : AsyncStateMachineBase
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        protected override Task OnState1Entered(Trigger trigger, State1Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState1Entered(StartTrigger trigger, State1Choices choices) => Task.Run(() => LogTransition(typeof(StartTrigger)));

        protected override Task OnState1Exited(Trigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState2Entered(Trigger trigger, State2Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override void OnState2Entered(CheckTrigger trigger, State2Choices choices) => LogTransition(typeof(CheckTrigger));

        protected override Task OnState2Entered(ContinueTrigger trigger, State2Choices choices) => Task.Run(() => LogTransition(typeof(ContinueTrigger)));

        protected override void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override Task OnState2Exited(ContinueTrigger trigger) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override Task OnState3Entered(Trigger trigger, State3Choices choices) => Task.Run(() => LogTransition(typeof(Trigger)));

        protected override async Task OnState3Entered(ContinueTrigger trigger, State3Choices choices)
        {
            LogTransition(typeof(ContinueTrigger));
            await ContinueAsync().ConfigureAwait(false);
        }

        protected override void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        protected override void OnState4Entered(Trigger trigger, State4Choices choices) => LogTransition(typeof(Trigger));

        protected override void OnState4Entered(ContinueTrigger trigger, State4Choices choices) => LogTransition(typeof(ContinueTrigger));
        protected override void OnState4Exited(Trigger trigger) => LogTransition(typeof(Trigger));
    }
}
