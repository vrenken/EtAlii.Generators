namespace EtAlii.Generators.MicroMachine.Tests
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
            Assert.Equal(6, stateMachine.Transitions.Count);
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(Continue1Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i]);
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
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Continue2Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i]);
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
            Assert.Equal(7, stateMachine.Transitions.Count);
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Continue3Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState3Entered(ContinueTrigger trigger)", stateMachine.Transitions[i]);
        }
    }
}
