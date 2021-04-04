namespace EtAlii.Generators.Stateless.Tests
{
    using Xunit;

    public class SubStateStateMachineTests
    {
        [Fact]
        public void SubStateStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new SubStateStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void SubStateStateMachine_Run_01()
        {
            // Arrange.
            var stateMachine = new SubStateStateMachine();

            // Act.
            stateMachine.Continue1();
            stateMachine.Continue();

            // Assert.
            Assert.Equal(4, stateMachine.Actions.Count);
            Assert.Equal("State 1 entered", stateMachine.Actions[0]);
            Assert.Equal("State 1 exited", stateMachine.Actions[1]);
            Assert.Equal("SuperState 1 entered", stateMachine.Actions[2]);
            Assert.Equal("SubState 1 entered", stateMachine.Actions[3]);
        }

        [Fact]
        public void SubStateStateMachine_Run_02()
        {
            // Arrange.
            var stateMachine = new SubStateStateMachine();

            // Act.
            stateMachine.Continue2();
            stateMachine.Continue();
            stateMachine.Start();

            // Assert.
            Assert.Equal(4, stateMachine.Actions.Count);
            Assert.Equal("State 2 entered", stateMachine.Actions[0]);
            Assert.Equal("State 2 exited", stateMachine.Actions[1]);
            Assert.Equal("SuperState 2 entered", stateMachine.Actions[2]);
            Assert.Equal("SubState 2 entered", stateMachine.Actions[3]);
        }

        [Fact]
        public void SubStateStateMachine_Run_03()
        {
            // Arrange.
            var stateMachine = new SubStateStateMachine();

            // Act.
            stateMachine.Continue3();
            stateMachine.Continue();

            // Assert.
            Assert.Equal(4, stateMachine.Actions.Count);
            Assert.Equal("State 3 entered", stateMachine.Actions[0]);
            Assert.Equal("State 3 exited", stateMachine.Actions[1]);
            Assert.Equal("SuperState 3 entered", stateMachine.Actions[2]);
            Assert.Equal("SubState 3 entered", stateMachine.Actions[3]);
        }
    }
}
