namespace EtAlii.Generators.MicroMachine.Tests
{
    using Xunit;

    public abstract class StateMachineTrigger
    {

    }

    public abstract class SimpleStateMachineTrigger : StateMachineTrigger
    {

    }

    public class SimpleStateMachineStartTrigger : SimpleStateMachineTrigger
    {

    }
    public class SimpleStateMachineContinueTrigger : SimpleStateMachineTrigger
    {

    }
    public class SimpleStateMachineCheckTrigger : SimpleStateMachineTrigger
    {

    }

    public abstract class SimpleStateMachineBase
    {
        public void Start()
        {

        }

        public void Continue()
        {

        }

        public void Check()
        {

        }
        protected virtual void OnState1Entered(SimpleStateMachineTrigger trigger) { }
        protected virtual void OnState1Entered(SimpleStateMachineStartTrigger trigger) { }

        protected virtual void OnState1Exited(SimpleStateMachineContinueTrigger trigger) { }
        protected virtual void OnState1Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState2Entered(SimpleStateMachineContinueTrigger trigger) { }
        protected virtual void OnState2Entered(SimpleStateMachineCheckTrigger trigger) { }
        protected virtual void OnState2Entered(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState2Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState3Entered(SimpleStateMachineTrigger trigger) { }


        protected virtual void OnState3Entered(SimpleStateMachineContinueTrigger trigger) { }

        protected virtual void OnState3Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState4Entered(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState4Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState4Entered(SimpleStateMachineContinueTrigger trigger) { }

    }
}
