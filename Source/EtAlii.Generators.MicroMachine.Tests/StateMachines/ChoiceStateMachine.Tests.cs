namespace EtAlii.Generators.MicroMachine.Tests
{
    using Xunit;

    public class ChoiceStateMachineTests
    {
        [Fact]
        public void ChoiceStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new ChoiceStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void ChoiceStateMachine_Run_Up()
        {
            // Arrange.
            var stateMachine = new ChoiceStateMachine();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Up("Hello");

            // Assert.
            var i = 0;
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(UpTrigger trigger)", stateMachine.Transitions[i]);
            Assert.Equal(8, stateMachine.Transitions.Count);
        }

        [Fact]
        public void ChoiceStateMachine_Run_Down()
        {
            // Arrange.
            var stateMachine = new ChoiceStateMachine();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Down();

            // Assert.
            var i = 0;
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState4Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState4Entered(DownTrigger trigger)", stateMachine.Transitions[i]);
            Assert.Equal(8, stateMachine.Transitions.Count);
        }

    }
}
