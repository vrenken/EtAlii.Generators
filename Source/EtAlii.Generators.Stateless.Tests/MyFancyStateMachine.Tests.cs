namespace EtAlii.Generators.Stateless.Tests
{
    using Xunit;

    public class MyFancyStateMachineTests
    {
        [Fact]
        public void MyFancyStateMachineTests_Create()
        {
            // Arrange.

            // Act.
            var stateMachine = new MyFancyStateMachine();

            // Assert.
            Assert.NotNull(stateMachine);
        }

        [Fact]
        public void MyFancyStateMachineTests_Start()
        {
            // Arrange.
            var stateMachine = new MyFancyStateMachine();

            // Act.
            stateMachine.Start("John");

            // Assert.
        }
    }
}
