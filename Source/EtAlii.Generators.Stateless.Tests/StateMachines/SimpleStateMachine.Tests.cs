namespace EtAlii.Generators.Stateless.Tests
{
    using Xunit;

    public class SimpleStateMachineTests
    {
        [Fact]
        public void SimpleStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new SimpleStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void SimpleStateMachine_Start()
        {
            // Arrange.
            var stateMachine = new SimpleStateMachine();

            // Act.
            stateMachine.Start();

            // Assert.
        }

        [Fact]
        public void SimpleStateMachine_Run()
        {
            // Arrange.
            var stateMachine = new SimpleStateMachine();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Check();
            stateMachine.Continue();

            // Assert.
            var i = 0;
            Assert.Equal(12, stateMachine.Actions.Count);
            Assert.Equal("State 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 1 entered from start trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 1 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered from continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("Check trigger called", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered from Continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 3 exited", stateMachine.Actions[i++]);
            Assert.Equal("State 4 entered", stateMachine.Actions[i]);
        }
    }
}
