namespace EtAlii.Generators.MicroMachine.Tests
{
    using Xunit;

    public abstract class SubStateStateMachineBase
    {
        public void Start()
        {

        }

        public void Continue()
        {

        }

        public void Continue1()
        {

        }

        public void Continue2()
        {

        }

        public void Continue3()
        {

        }


        public void Check()
        {

        }
        protected abstract void OnState1Entered();

        protected abstract void OnState1Exited();

        protected abstract void OnState2Entered();

        protected abstract void OnState2Exited();

        protected abstract void OnState3Entered();

        protected abstract void OnState3Exited();

        protected abstract void OnSuperState1Entered();

        protected abstract void OnSuperState1Exited();

        protected abstract void OnSuperState2Entered();

        protected abstract void OnSuperState2Exited();

        protected abstract void OnSubState1Entered();

        protected abstract void OnSubState2Entered();

        protected abstract void OnSubState3Entered();

        protected abstract void OnSuperState3Entered();

        protected abstract void OnSuperState3Exited();

        protected abstract void OnState1EnteredFromContinue1Trigger();

        protected abstract void OnState2EnteredFromContinue2Trigger();

        protected abstract void OnState3EnteredFromContinue3Trigger();

        protected abstract void OnSubState1Exited();

        protected abstract void OnSubState1EnteredFrom_BeginToSubState1Trigger();

        protected abstract void OnSubState2Exited();

        protected abstract void OnSubState2EnteredFromStartTrigger();

        protected abstract void OnSubState3Exited();

        protected abstract void OnSubState3EnteredFromContinueTrigger();

        protected abstract void OnSuperState1EnteredFromContinueTrigger();

        protected abstract void OnSuperState2EnteredFromContinueTrigger();
    }
}
