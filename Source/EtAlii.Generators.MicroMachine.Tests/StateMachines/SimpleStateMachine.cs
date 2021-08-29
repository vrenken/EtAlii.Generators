namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Collections.Generic;

    public class SimpleStateMachine : SimpleStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override void OnState1Entered(Trigger trigger) => Actions.Add("State 1 entered");

        protected override void OnState1Entered(StartTrigger trigger) => Actions.Add("State 1 entered by Start trigger");
        protected override void OnState1Exited(Trigger trigger) => Actions.Add("State 1 exited");

        protected override void OnState1Exited(ContinueTrigger trigger) => Actions.Add("State 1 exited by Continue trigger");


        protected override void OnState2Entered(Trigger trigger) => Actions.Add("State 2 entered");

        protected override void OnState2Entered(ContinueTrigger trigger) => Actions.Add("State 2 entered by Continue trigger");
        protected override void OnState2Exited(Trigger trigger) => Actions.Add("State 2 exited");
        protected override void OnState2Exited(CheckTrigger trigger) => Actions.Add("State 2 exited by Check trigger");
        protected override void OnState2Entered(CheckTrigger trigger) => Actions.Add("State 2 entered by Check trigger");

        protected override void OnState2Exited(ContinueTrigger trigger) => Actions.Add("State 2 exited by Continue trigger");


        protected override void OnState3Entered(Trigger trigger) => Actions.Add("State 3 entered");

        protected override void OnState3Entered(ContinueTrigger trigger)
        {
            Actions.Add("State 3 entered by Continue trigger");
            Continue();
        }


        protected override void OnState3Exited(Trigger trigger) => Actions.Add("State 3 exited");
        protected override void OnState3Exited(ContinueTrigger trigger) => Actions.Add("State 3 exited by Continue trigger");

        protected override void OnState4Entered(Trigger trigger) => Actions.Add("State 4 entered");

        protected override void OnState4Entered(ContinueTrigger trigger) => Actions.Add("State 4 entered by Continue trigger");


        protected override void OnState4Exited(Trigger trigger) => Actions.Add("State 4 exited");

    }
}
