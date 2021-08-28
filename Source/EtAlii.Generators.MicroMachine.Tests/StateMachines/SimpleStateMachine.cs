namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Collections.Generic;

    public class SimpleStateMachine : SimpleStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override void OnState1Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 1 entered");

        protected override void OnState1Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 1 exited");

        protected override void OnState2Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 2 entered");

        protected override void OnState2Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 2 exited");

        protected override void OnState3Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 3 entered");

        protected override void OnState3Entered(SimpleStateMachineContinueTrigger trigger)
        {
            Actions.Add("State 3 entered from Continue trigger");
            Continue();
        }

        protected override void OnState2Entered(SimpleStateMachineCheckTrigger trigger) => Actions.Add("Check trigger called");

        protected override void OnState3Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 3 exited");

        protected override void OnState4Entered(SimpleStateMachineTrigger trigger) => Actions.Add("State 4 entered");

        protected override void OnState1Entered(SimpleStateMachineStartTrigger trigger) => Actions.Add("State 1 entered from start trigger");

        protected override void OnState2Entered(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 2 entered from continue trigger");

        protected override void OnState4Exited(SimpleStateMachineTrigger trigger) => Actions.Add("State 4 exited");

        protected override void OnState4Entered(SimpleStateMachineContinueTrigger trigger) => Actions.Add("State 4 entered from continue trigger");
    }
}
