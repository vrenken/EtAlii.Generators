namespace EtAlii.Generators.MicroMachine.Tests
{
    using EtAlii.Generators.MicroMachine.Tests.Nested;
    using Xunit;

    public class ParameterStateMachineTests
    {
        [Fact]
        public void ParameterStateMachine_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new ParameterStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void ParameterStateMachine_Run_01()
        {
            // Arrange.
            var stateMachine = new ParameterStateMachine();

            // Act.
            stateMachine.Continue1();
            stateMachine.Activate1("title 1", 42);

            // Assert.
            var i = 0;
            Assert.Equal(6, stateMachine.Transitions.Count);
            Assert.Equal("OnState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Entered(Continue1Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Activate1Trigger: title 1, 42)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState1Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnNextState1Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnNextState1Entered(Activate1Trigger: title 1, 42)", stateMachine.Transitions[i]);
        }

        [Fact]
        public void ParameterStateMachine_Run_02()
        {
            // Arrange.
            var stateMachine = new ParameterStateMachine();

            // Act.
            stateMachine.Continue2();
            stateMachine.Activate2("My new string", 42, 2.3234f);

            // Assert.
            var i = 0;
            Assert.Equal(6, stateMachine.Transitions.Count);
            Assert.Equal("OnState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Entered(Continue2Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Activate2Trigger: My new string, 42, 2.3234)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState2Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnNextState2Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnNextState2Entered(Activate2Trigger: My new string, 42, 2.3234)", stateMachine.Transitions[i]);
        }

        [Fact]
        public void ParameterStateMachine_Run_03()
        {
            // Arrange.
            var stateMachine = new ParameterStateMachine();
            var customer = new Customer(12, "Jane", "Doe");
            var project = new Project(33, "Test project 3");

            // Act.
            stateMachine.Continue3();
            stateMachine.Activate3(customer, project);

            // Assert.
            var i = 0;
            Assert.Equal(6, stateMachine.Transitions.Count);
            Assert.Equal("OnState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Entered(Continue3Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(Activate3Trigger: 12, 33)", stateMachine.Transitions[i++]);
            Assert.Equal("OnState3Exited(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnNextState3Entered(Trigger trigger)", stateMachine.Transitions[i++]);
            Assert.Equal("OnNextState3Entered(Activate3Trigger: 12, 33)", stateMachine.Transitions[i]);
        }
    }
}
