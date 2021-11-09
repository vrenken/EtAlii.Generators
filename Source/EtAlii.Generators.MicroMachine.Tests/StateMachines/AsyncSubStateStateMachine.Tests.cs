namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AsyncSubStateStateMachineTests
    {
        [Fact]
        public void AsyncSubStateStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new AsyncSubStateStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public async Task AsyncSubStateStateMachine_Run_01()
        {
            // Arrange.
            var stateMachine = new AsyncSubStateStateMachine();

            // Act.
            await stateMachine.Continue1Async().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(Continue1Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }

        [Fact]
        public async Task AsyncSubStateStateMachine_Run_02()
        {
            // Arrange.
            var stateMachine = new AsyncSubStateStateMachine();

            // Act.
            await stateMachine.Continue2Async().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);
            await stateMachine.StartAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Continue2Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }

        [Fact]
        public async Task AsyncSubStateStateMachine_Run_03()
        {
            // Arrange.
            var stateMachine = new AsyncSubStateStateMachine();

            // Act.
            await stateMachine.Continue3Async().ConfigureAwait(false);
            await stateMachine.ContinueAsync().ConfigureAwait(false);

            // Assert.
            var i = 0;
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Continue3Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSuperState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState3Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }
    }
}
