namespace EtAlii.Generators.MicroMachine.Tests
{
    using Xunit;

    public class SimplePartialStateMachineTests
    {
        [Fact]
        public void SimplePartialStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new SimplePartialStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void SimplePartialStateMachine_Start()
        {
            // Arrange.
            var stateMachine = new SimplePartialStateMachine();

            // Act.
            stateMachine.Start();

            // Assert.
        }

        [Fact]
        public void SimplePartialStateMachine_Run()
        {
            // Arrange.
            var stateMachine = new SimplePartialStateMachine();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Check();
            stateMachine.Continue();

            // Assert.
            var i = 0;
            Assert.Equal(18, stateMachine.Actions.Count);
            Assert.Equal("State 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 1 entered by Start trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 1 exited by Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 1 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered by Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited by Check trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered by Check trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited by Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered by Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 3 exited by Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 3 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 4 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 4 entered by Continue trigger", stateMachine.Actions[i]);
        }
    }
}
