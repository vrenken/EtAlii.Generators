namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncSubStateStateMachine : AsyncSubStateStateMachineBase
    {
        public List<string> Actions { get; } = new();

        public List<string> UnhandledTriggers { get; } = new();

        public AsyncSubStateStateMachine()
        {
            StateMachine.OnUnhandledTrigger((state, trigger) => UnhandledTriggers.Add($"{state}-{trigger}"));
            StateMachine.OnUnhandledTriggerAsync((state, trigger) => Task.Run(() => UnhandledTriggers.Add($"{state}-{trigger}")));
        }

        protected override Task OnState1Entered() => Task.Run(() => Actions.Add("State 1 entered"));

        protected override Task OnState1Exited() => Task.Run(() => Actions.Add("State 1 exited"));

        protected override Task OnState2Entered() => Task.Run(() => Actions.Add("State 2 entered"));

        protected override Task OnState2Exited() => Task.Run(() => Actions.Add("State 2 exited"));

        protected override Task OnState3Entered() => Task.Run(() => Actions.Add("State 3 entered"));

        protected override Task OnState3Exited() => Task.Run(() => Actions.Add("State 3 exited"));

        protected override Task OnSuperState1Entered() => Task.Run(() => Actions.Add("SuperState 1 entered"));

        protected override Task OnSuperState1Exited() => Task.Run(() => Actions.Add("SuperState 1 exited"));

        protected override Task OnSuperState2Entered() => Task.Run(() => Actions.Add("SuperState 2 entered"));

        protected override Task OnSuperState2Exited() => Task.Run(() => Actions.Add("SuperState 2 exited"));

        protected override Task OnSubState1Entered() => Task.Run(() => Actions.Add("SubState 1 entered"));

        protected override Task OnSubState2Entered() => Task.Run(() => Actions.Add("SubState 2 entered"));

        protected override Task OnSubState3Entered() => Task.Run(() => Actions.Add("SubState 3 entered"));

        protected override Task OnSuperState3Entered() => Task.Run(() => Actions.Add("SuperState 3 entered"));

        protected override void OnSuperState3Exited() => Actions.Add("SuperState 3 exited");

        protected override Task OnState1EnteredFromContinue1Trigger() => Task.Run(() => Actions.Add("State 1 entered from continue 1 trigger"));

        protected override Task OnState2EnteredFromContinue2Trigger() => Task.Run(() => Actions.Add("State 2 entered from continue 2 trigger"));

        protected override Task OnState3EnteredFromContinue3Trigger() => Task.Run(() => Actions.Add("State 3 entered from continue 3 trigger"));

        protected override void OnSubState1Exited() => Actions.Add("SubState 1 exited");

        protected override Task OnSubState1EnteredFrom_BeginToSubState1Trigger() => Task.Run(() => Actions.Add("SubState 1 entered from _BeginToSubState trigger"));

        protected override void OnSubState2Exited() => Actions.Add("SubState 2 exited");

        protected override Task OnSubState2EnteredFromStartTrigger() => Task.Run(() => Actions.Add("SubState 2 entered from start trigger"));

        protected override void OnSubState3Exited() => Actions.Add("SubState 3 exited");

        protected override Task OnSubState3EnteredFromContinueTrigger() => Task.Run(() => Actions.Add("SubState 3 entered from continue trigger"));

        protected override Task OnSuperState1EnteredFromContinueTrigger() => Task.Run(() => Actions.Add("SuperState 1 entered from continue trigger"));

        protected override Task OnSuperState2EnteredFromContinueTrigger() => Task.Run(() => Actions.Add("SuperState 2 entered from continue trigger"));
    }
}
