namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using Xunit;

    public class MyNestedStateMachine1Tests
    {
        [Fact]
        public void MyNestedStateMachine1_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new MyNestedStateMachine1();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void MyNestedStateMachine1_Start()
        {
            // Arrange.
            var stateMachine = new MyNestedStateMachine1();

            // Act.
            stateMachine.Start();

            // Assert.
        }

        [Fact]
        public void MyNestedStateMachine1_Run_And_Exit_1()
        {
            // Arrange.
            var stateMachine = new MyNestedStateMachine1();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Exit();

            // Assert.
            var i = 0;
            Assert.Equal(18, stateMachine.Transitions.Count);
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }


        [Fact]
        public void MyNestedStateMachine1_Run_And_Exit_2()
        {
            // Arrange.
            var stateMachine = new MyNestedStateMachine1();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Continue();
            stateMachine.Exit();

            // Assert.
            var i = 0;
            Assert.Equal(18, stateMachine.Transitions.Count);
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }

        [Fact]
        public void MyNestedStateMachine1_Run_And_Check_1()
        {
            // Arrange.
            var stateMachine = new MyNestedStateMachine1();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Check();

            // Assert.
            var i = 0;
            Assert.Equal(18, stateMachine.Transitions.Count);
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }

        [Fact]
        public void MyNestedStateMachine1_Run_And_Check_2()
        {
            // Arrange.
            var stateMachine = new MyNestedStateMachine1();

            // Act.
            stateMachine.Start();
            stateMachine.Continue();
            stateMachine.Continue();
            stateMachine.Check();

            // Assert.
            var i = 0;
            Assert.Equal(18, stateMachine.Transitions.Count);
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(StartTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnSubState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(ContinueTrigger trigger)", stateMachine.Transitions[i++]);
            Assert.ThrowsAny<Exception>(() => stateMachine.Transitions[i++]);
        }
    }
}
