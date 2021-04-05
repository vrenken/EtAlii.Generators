namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;

    public class SimpleStateMachine : SimpleStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override void OnState1Entered() => Actions.Add("State 1 entered");

        protected override void OnState1Exited() => Actions.Add("State 1 exited");

        protected override void OnState2Entered() => Actions.Add("State 2 entered");

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override void OnState3Entered() => Actions.Add("State 3 entered");

        protected override void OnState3EnteredFromContinueTrigger()
        {
            Actions.Add("State 3 entered from Continue trigger");
            Continue();
        }

        protected override void OnState2InternalCheckTrigger() => Actions.Add("Check trigger called");

        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnState4Entered() => Actions.Add("State 4 entered");
    }
}
