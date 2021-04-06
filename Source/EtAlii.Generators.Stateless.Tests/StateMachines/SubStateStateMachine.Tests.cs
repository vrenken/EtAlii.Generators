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
            var i = 0;
            Assert.Equal(6, stateMachine.Actions.Count);
            Assert.Equal("State 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 1 entered from continue 1 trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 1 exited", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 1 entered", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 1 entered from continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("SubState 1 entered", stateMachine.Actions[i++]);
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
            var i = 0;
            Assert.Equal(7, stateMachine.Actions.Count);
            Assert.Equal("State 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 2 entered from continue 2 trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 2 exited", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 2 entered", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 2 entered from continue trigger", stateMachine.Actions[i++]);
            Assert.Equal("SubState 2 entered", stateMachine.Actions[i++]);
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
            var i = 0;
            Assert.Equal(6, stateMachine.Actions.Count);
            Assert.Equal("State 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("State 3 entered from continue 3 trigger", stateMachine.Actions[i++]);
            Assert.Equal("State 3 exited", stateMachine.Actions[i++]);
            Assert.Equal("SuperState 3 entered", stateMachine.Actions[i++]);
            Assert.Equal("SubState 3 entered", stateMachine.Actions[i++]);
        }
    }
}
