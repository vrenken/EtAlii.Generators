namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
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
        }

        [Fact]
        public async Task AsyncStateMachine_Start()
        {
            // Arrange.
            var stateMachine = new AsyncStateMachine();

            // Act.
            await stateMachine.StartAsync().ConfigureAwait(false);

            // Assert.
            Assert.True(stateMachine.Transitions.Count > 0);
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
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
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
            Assert.Equal("OnState3Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState4Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState4Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }
    }
}
