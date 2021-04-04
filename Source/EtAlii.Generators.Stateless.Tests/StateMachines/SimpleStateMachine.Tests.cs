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
            Assert.True(stateMachine.Actions.Count == 9);
            Assert.Equal("State 1 entered", stateMachine.Actions[0]);
            Assert.Equal("State 1 exited", stateMachine.Actions[1]);
            Assert.Equal("State 2 entered", stateMachine.Actions[2]);
            Assert.Equal("Check trigger called", stateMachine.Actions[3]);
            Assert.Equal("State 2 exited", stateMachine.Actions[4]);
            Assert.Equal("State 3 entered", stateMachine.Actions[5]);
            Assert.Equal("State 3 entered from Continue trigger", stateMachine.Actions[6]);
            Assert.Equal("State 3 exited", stateMachine.Actions[7]);
            Assert.Equal("State 4 entered", stateMachine.Actions[8]);
        }
    }
}
