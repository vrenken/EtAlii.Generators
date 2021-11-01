namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;

    public class ChoiceStateMachine : ChoiceStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override void OnState1Entered(State1EventArgs e) => Actions.Add("State 1 entered");

        protected override void OnState1Exited() => Actions.Add("State 1 exited");

        protected override void OnState2Entered(State2EventArgs e) => Actions.Add("State 2 entered");

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override void OnState3Entered(State3EventArgs e) => Actions.Add("State 3 entered");

        protected override void OnState3EnteredFromUpTrigger(State3EventArgs e, string message)
        {
            Actions.Add($"State 3 entered from Up trigger: {message}");
            Continue();
        }


        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnState4Entered(State4EventArgs e) => Actions.Add("State 4 entered");

        protected override void OnState1EnteredFromStartTrigger(State1EventArgs e) => Actions.Add("State 1 entered from start trigger");

        protected override void OnState2EnteredFromContinueTrigger(State2EventArgs e) => Actions.Add("State 2 entered from continue trigger");

        protected override void OnState4Exited() => Actions.Add("State 4 exited");

        protected override void OnState4EnteredFromDownTrigger(State4EventArgs e)
        {
        }
    }
}
