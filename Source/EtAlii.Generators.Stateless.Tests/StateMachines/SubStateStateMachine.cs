namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SubStateStateMachine : SubStateStateMachineBase
    {
        public List<string> Actions { get; } = new();

        public List<string> UnhandledTriggers { get; } = new();

        public SubStateStateMachine()
        {
            StateMachine.OnUnhandledTrigger((state, trigger) => UnhandledTriggers.Add($"{state}-{trigger}"));
            StateMachine.OnUnhandledTriggerAsync((state, trigger) => Task.Run(() => UnhandledTriggers.Add($"{state}-{trigger}")));
        }

        protected override void OnState1Entered(State1EventArgs e) => Actions.Add("State 1 entered");

        protected override void OnState1Exited() => Actions.Add("State 1 exited");

        protected override void OnState2Entered(State2EventArgs e) => Actions.Add("State 2 entered");

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override void OnState3Entered(State3EventArgs e) => Actions.Add("State 3 entered");

        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnSuperState1Entered(SuperState1EventArgs e) => Actions.Add("SuperState 1 entered");

        protected override void OnSuperState1Exited() => Actions.Add("SuperState 1 exited");

        protected override void OnSuperState2Entered(SuperState2EventArgs e) => Actions.Add("SuperState 2 entered");

        protected override void OnSuperState2Exited() => Actions.Add("SuperState 2 exited");

        protected override void OnSubState1Entered(SubState1EventArgs e) => Actions.Add("SubState 1 entered");

        protected override void OnSubState2Entered(SubState2EventArgs e) => Actions.Add("SubState 2 entered");

        protected override void OnSubState3Entered(SubState3EventArgs e) => Actions.Add("SubState 3 entered");

        protected override void OnSuperState3Entered(SuperState3EventArgs e) => Actions.Add("SuperState 3 entered");

        protected override void OnSuperState3Exited() => Actions.Add("SuperState 3 exited");

        protected override void OnState1EnteredFromContinue1Trigger(State1EventArgs e) => Actions.Add("State 1 entered from continue 1 trigger");

        protected override void OnState2EnteredFromContinue2Trigger(State2EventArgs e) => Actions.Add("State 2 entered from continue 2 trigger");

        protected override void OnState3EnteredFromContinue3Trigger(State3EventArgs e) => Actions.Add("State 3 entered from continue 3 trigger");

        protected override void OnSubState1Exited() => Actions.Add("SubState 1 exited");

        protected override void OnSubState1EnteredFrom_BeginToSubState1Trigger(SubState1EventArgs e) => Actions.Add("SubState 1 entered from _BeginToSubState trigger");

        protected override void OnSubState2Exited() => Actions.Add("SubState 2 exited");

        protected override void OnSubState2EnteredFromStartTrigger(SubState2EventArgs e) => Actions.Add("SubState 2 entered from start trigger");

        protected override void OnSubState3Exited() => Actions.Add("SubState 3 exited");

        protected override void OnSubState3EnteredFromContinueTrigger(SubState3EventArgs e) => Actions.Add("SubState 3 entered from continue trigger");

        protected override void OnSuperState1EnteredFromContinueTrigger(SuperState1EventArgs e) => Actions.Add("SuperState 1 entered from continue trigger");

        protected override void OnSuperState2EnteredFromContinueTrigger(SuperState2EventArgs e) => Actions.Add("SuperState 2 entered from continue trigger");
    }
}
