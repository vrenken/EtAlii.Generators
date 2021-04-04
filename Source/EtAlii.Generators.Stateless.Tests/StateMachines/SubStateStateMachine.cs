namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;

    public class SubStateStateMachine : SubStateStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override void OnState1Entered() => Actions.Add("State 1 entered");

        protected override void OnState1Exited() => Actions.Add("State 1 exited");

        protected override void OnState2Entered() => Actions.Add("State 2 entered");

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override void OnState3Entered() => Actions.Add("State 3 entered");

        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnSuperState1Entered() => Actions.Add("SuperState 1 entered");

        protected override void OnSuperState1Exited() => Actions.Add("SuperState 1 exited");

        protected override void OnSuperState2Entered() => Actions.Add("SuperState 2 entered");

        protected override void OnSuperState2Exited() => Actions.Add("SuperState 2 exited");

        protected override void OnSubState1Entered() => Actions.Add("SubState 1 entered");

        protected override void OnSubState2Entered() => Actions.Add("SubState 2 entered");

        protected override void OnSubState3Entered() => Actions.Add("SubState 3 entered");

        protected override void OnSuperState3Entered() => Actions.Add("SuperState 3 entered");

        protected override void OnSuperState3Exited() => Actions.Add("SuperState 3 exited");
    }
}
