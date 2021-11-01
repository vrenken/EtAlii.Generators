namespace EtAlii.Generators.Stateless.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncStateMachine : AsyncStateMachineBase
    {
        public List<string> Actions { get; } = new();

        public List<string> UnhandledTriggers { get; } = new();

        public AsyncStateMachine()
        {
            StateMachine.OnUnhandledTrigger((state, trigger) => UnhandledTriggers.Add($"{state}-{trigger}"));
            StateMachine.OnUnhandledTriggerAsync((state, trigger) => Task.Run(() => UnhandledTriggers.Add($"{state}-{trigger}")));
        }

        protected override Task OnState1Entered(State1EventArgs e) => Task.Run(() => Actions.Add("State 1 entered"));

        protected override Task OnState1Exited() => Task.Run(() => Actions.Add("State 1 exited"));

        protected override Task OnState2Entered(State2EventArgs e) => Task.Run(() => Actions.Add("State 2 entered"));

        protected override void OnState2Exited() => Actions.Add("State 2 exited");

        protected override Task OnState3Entered(State3EventArgs e) => Task.Run(() => Actions.Add("State 3 entered"));

        protected override async Task OnState3EnteredFromContinueTrigger(State3EventArgs e)
        {
            Actions.Add("State 3 entered from Continue trigger");
            await ContinueAsync().ConfigureAwait(false);
        }

        protected override void OnState2InternalCheckTrigger() => Actions.Add("Check trigger called");

        protected override void OnState3Exited() => Actions.Add("State 3 exited");

        protected override void OnState4Entered(State4EventArgs e) => Actions.Add("State 4 entered");

        protected override Task OnState1EnteredFromStartTrigger(State1EventArgs e) => Task.Run(() => Actions.Add("State 1 entered from start trigger"));

        protected override Task OnState2EnteredFromContinueTrigger(State2EventArgs e) => Task.Run(() => Actions.Add("State 2 entered from start trigger"));

        protected override void OnState4Exited() => Actions.Add("State 4 exited");

        protected override void OnState4EnteredFromContinueTrigger(State4EventArgs e) => Actions.Add("State 4 entered from continue trigger");
    }
}
