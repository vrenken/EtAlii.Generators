namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncStateMachine : AsyncStateMachineBase
    {
        public List<string> Actions { get; } = new();

        protected override Task OnState1EnteredAsync() => Task.Run(() => Actions.Add("State 1 entered"));

        protected override Task OnState1ExitedAsync() => Task.Run(() => Actions.Add("State 1 exited"));

        protected override Task OnState2EnteredAsync() => Task.Run(() => Actions.Add("State 2 entered"));

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override Task OnState3EnteredAsync() => Task.Run(() => Actions.Add("State 3 entered"));

        protected override Task OnState3EnteredFromContinueTrigger()
        {
            Actions.Add("State 3 entered from Continue trigger");
            Continue();
            return Task.CompletedTask;
        }

        protected override void OnState2InternalCheckTrigger() => Actions.Add("Check trigger called");

        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnState4Entered() => Actions.Add("State 4 entered");
    }
}
