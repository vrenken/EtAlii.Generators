namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Collections.Generic;

    public class SimpleStateMachine : SimpleStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override void OnState1Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 1 entered");

        protected override void OnState1Entered(SimpleStateMachineStartTrigger trigger) => Actions.Add("State 1 entered by Start trigger");
        protected override void OnState1Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 1 exited");

        protected override void OnState1Exited(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 1 exited by Continue trigger");


        protected override void OnState2Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 2 entered");

        protected override void OnState2Entered(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 2 entered by Continue trigger");
        protected override void OnState2Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 2 exited");
        protected override void OnState2Exited(SimpleStateMachineCheckTrigger trigger) => Actions.Add("State 2 exited by Check trigger");
        protected override void OnState2Entered(SimpleStateMachineCheckTrigger trigger) => Actions.Add("State 2 entered by Check trigger");

        protected override void OnState2Exited(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 2 exited by Continue trigger");


        protected override void OnState3Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 3 entered");

        protected override void OnState3Entered(SimpleStateMachineContinueTrigger trigger)
        {
            Actions.Add("State 3 entered by Continue trigger");
            Continue();
        }


        protected override void OnState3Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 3 exited");
        protected override void OnState3Exited(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 3 exited by Continue trigger");

        protected override void OnState4Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 4 entered");

        protected override void OnState4Entered(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 4 entered by Continue trigger");


        protected override void OnState4Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 4 exited");

    }
}
