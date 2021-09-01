namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Collections.Generic;

    public partial class SimplePartialStateMachine
    {
        public List<string> Actions { get; } = new();

        partial void OnState1Entered(Trigger trigger) => Actions.Add("State 1 entered");

        partial void OnState1Entered(StartTrigger trigger) => Actions.Add("State 1 entered by Start trigger");
        partial void OnState1Exited(Trigger trigger) => Actions.Add("State 1 exited");

        partial void OnState1Exited(ContinueTrigger trigger) => Actions.Add("State 1 exited by Continue trigger");


        partial void OnState2Entered(Trigger trigger) => Actions.Add("State 2 entered");

        partial void OnState2Entered(ContinueTrigger trigger) => Actions.Add("State 2 entered by Continue trigger");
        partial void OnState2Exited(Trigger trigger) => Actions.Add("State 2 exited");
        partial void OnState2Exited(CheckTrigger trigger) => Actions.Add("State 2 exited by Check trigger");
        partial void OnState2Entered(CheckTrigger trigger) => Actions.Add("State 2 entered by Check trigger");

        partial void OnState2Exited(ContinueTrigger trigger) => Actions.Add("State 2 exited by Continue trigger");


        partial void OnState3Entered(Trigger trigger) => Actions.Add("State 3 entered");

        partial void OnState3Entered(ContinueTrigger trigger)
        {
            Actions.Add("State 3 entered by Continue trigger");
            Continue();
        }

        partial void OnState3Exited(Trigger trigger) => Actions.Add("State 3 exited");
        partial void OnState3Exited(ContinueTrigger trigger) => Actions.Add("State 3 exited by Continue trigger");

        partial void OnState4Entered(Trigger trigger) => Actions.Add("State 4 entered");
        partial void OnState4Entered(ContinueTrigger trigger) => Actions.Add("State 4 entered by Continue trigger");
        partial void OnState4Exited(Trigger trigger) => Actions.Add("State 4 exited");
    }
}
