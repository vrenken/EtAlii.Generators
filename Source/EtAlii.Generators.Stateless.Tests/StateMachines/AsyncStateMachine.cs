namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncStateMachine : AsyncStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override Task OnState1Entered() => Task.Run(() => Actions.Add("State 1 entered"));

        protected override Task OnState1Exited() => Task.Run(() => Actions.Add("State 1 exited"));

        protected override Task OnState2Entered() => Task.Run(() => Actions.Add("State 2 entered"));

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override Task OnState3Entered() => Task.Run(() => Actions.Add("State 3 entered"));

        protected override Task OnState3EnteredFromContinueTrigger()
        {
            Actions.Add("State 3 entered from Continue trigger");
            Continue();
            return Task.CompletedTask;
        }

        protected override void OnState2InternalCheckTrigger() => Actions.Add("Check trigger called");

        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnState4Entered() => Actions.Add("State 4 entered");

        protected override Task OnState1EnteredFromStartTrigger() => Task.Run(() => Actions.Add("State 1 entered from start trigger"));

        protected override Task OnState2EnteredFromContinueTrigger() => Task.Run(() => Actions.Add("State 2 entered from start trigger"));

        protected override void OnState4Exited() => Actions.Add("State 4 exited");

        protected override void OnState4EnteredFromContinueTrigger() => Actions.Add("State 4 entered from continue trigger");
    }
}
