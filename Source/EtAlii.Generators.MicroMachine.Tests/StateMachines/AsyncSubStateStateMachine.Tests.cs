namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Threading.Tasks;
    using Xunit;

    public class AsyncSubStateStateMachineTests
    {
        [Fact]
        public void AsyncSubStateStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new AsyncSubStateStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
            Assert.Empty(stateMachine.UnhandledTriggers);
        }

        [Fact]
        public async Task AsyncSubStateStateMachine_Run_01()
        {
            // Arrange.
            var stateMachine = new AsyncSubStateStateMachine();

            // Act.
            await stateMachine.Continue1Async().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Empty(stateMachine.UnhandledTriggers);
            Assert.Equal(6, stateMachine.Actions.Count);
            Assert.Equal("State 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 1 entered from continue 1 trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 1 exited", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 1 entered from continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("SubState 1 entered", stateMachine.Actions[i]);
        }

        [Fact(Skip = "This test makes a bug visible in the MicroMachine project. We're about to propose a fix.")]
        public async Task AsyncSubStateStateMachine_Run_02()
        {
            // Arrange.
            var stateMachine = new AsyncSubStateStateMachine();

            // Act.
            await stateMachine.Continue2Async().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);
            await stateMachine.StartAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Empty(stateMachine.UnhandledTriggers);
            Assert.Equal(8, stateMachine.Actions.Count);
            Assert.Equal("State 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered from continue 2 trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 2 entered from continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("SubState 2 entered", stateMachine.Actions[i]);
        }

        [Fact]
        public async Task AsyncSubStateStateMachine_Run_03()
        {
            // Arrange.
            var stateMachine = new AsyncSubStateStateMachine();

            // Act.
            await stateMachine.Continue3Async().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Empty(stateMachine.UnhandledTriggers);
            Assert.Equal(6, stateMachine.Actions.Count);
            Assert.Equal("State 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered from continue 3 trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 3 exited", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("SubState 3 entered", stateMachine.Actions[i]);
        }
    }
}
