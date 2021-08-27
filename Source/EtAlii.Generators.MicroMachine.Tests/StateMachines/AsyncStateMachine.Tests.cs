namespace EtAlii.Generators.MicroMachine.Tests
{
    using System.Threading.Tasks;
    using Xunit;

    public class AsyncStateMachineTests
    {
        [Fact]
        public void AsyncStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new AsyncStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
            Assert.Empty(stateMachine.UnhandledTriggers);
        }

        [Fact]
        public async Task AsyncStateMachine_Start()
        {
            // Arrange.
            var stateMachine = new AsyncStateMachine();

            // Act.
            await stateMachine.StartAsync().ConfigureAwait(false);

            // Assert.
            Assert.Empty(stateMachine.UnhandledTriggers);
        }

        [Fact]
        public async Task AsyncStateMachine_Run()
        {
            // Arrange.
            var stateMachine = new AsyncStateMachine();

            // Act.
            await stateMachine.StartAsync().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);
            stateMachine.Check();
            await stateMachine.ContinueAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Empty(stateMachine.UnhandledTriggers);
            Assert.Equal(12, stateMachine.Actions.Count);
            Assert.Equal("State 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 1 entered from start trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 1 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered from start trigger", stateMachine.Actions[i++]);
            Assert.Equal("Check trigger called", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered from Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 3 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 4 entered", stateMachine.Actions[i]);
        }
    }
}
