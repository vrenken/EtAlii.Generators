namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
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
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(CheckTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(CheckTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState4Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState4Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }
    }
}
