namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
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
        public enum State
        {
            Undefined,
            State1,
            State2,
            State3,
            State4,
        }

        private State _state;
        private readonly Queue<Action> _transactions = new Queue<Action>();

        private void RunOrQueueTransition(Action transition)
        {
            var deQueue = _transactions.Count > 0;
            _transactions.Enqueue(transition);
            if (deQueue)
            {
                while(_transactions.TryDequeue(out var queuedTransaction))
                {
                    queuedTransaction();
                }
            }
        }

        private void StartTransition()
        {
            switch (_state)
            {
                case State.Undefined:
                    _state = State.State1;
                    SimpleStateMachineTrigger trigger = new SimpleStateMachineStartTrigger();
                    OnState1Entered(trigger);
                    OnState1Entered((SimpleStateMachineStartTrigger)trigger);
                    break;
                default:
                    throw new NotSupportedException($"Trigger Start is not supported in state {_state}");
            }
        }

        public void Start() => RunOrQueueTransition(StartTransition);

        private void ContinueTransition()
        {
            switch (_state)
            {
                case State.State1:
                    _state = State.State2;
                    var state1Trigger = new SimpleStateMachineContinueTrigger();
                    OnState1Exited(state1Trigger);
                    OnState1Exited((SimpleStateMachineTrigger)state1Trigger);
                    OnState2Entered((SimpleStateMachineTrigger)state1Trigger);
                    OnState2Entered(state1Trigger);
                    break;
                case State.State2:
                    _state = State.State3;
                    var state2Trigger = new SimpleStateMachineContinueTrigger();
                    OnState2Exited(state2Trigger);
                    OnState2Exited((SimpleStateMachineTrigger)state2Trigger);
                    OnState3Entered((SimpleStateMachineTrigger)state2Trigger);
                    OnState3Entered(state2Trigger);
                    break;
                case State.State3:
                    _state = State.State4;
                    var state3Trigger = new SimpleStateMachineContinueTrigger();
                    OnState3Exited(state3Trigger);
                    OnState3Exited((SimpleStateMachineTrigger)state3Trigger);
                    OnState4Entered((SimpleStateMachineTrigger)state3Trigger);
                    OnState4Entered(state3Trigger);
                    break;
                default:
                    throw new NotSupportedException($"Trigger Continue is not supported in state {_state}");
            }
        }
        public void Continue() => RunOrQueueTransition(ContinueTransition);

        private void CheckTransition()
        {
            switch (_state)
            {
                case State.State2:
                    var state2Trigger = new SimpleStateMachineCheckTrigger();
                    OnState2Exited(state2Trigger);
                    OnState2Exited((SimpleStateMachineTrigger)state2Trigger);
                    OnState2Entered((SimpleStateMachineTrigger)state2Trigger);
                    OnState2Entered(state2Trigger);
                    break;
                default:
                    throw new NotSupportedException($"Trigger Check is not supported in state {_state}");
            }
        }

        public void Check() => RunOrQueueTransition(CheckTransition);

        protected virtual void OnState1Entered(SimpleStateMachineTrigger trigger) { }
        protected virtual void OnState1Entered(SimpleStateMachineStartTrigger trigger) { }

        protected virtual void OnState1Exited(SimpleStateMachineContinueTrigger trigger) { }
        protected virtual void OnState1Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState2Entered(SimpleStateMachineContinueTrigger trigger) { }
        protected virtual void OnState2Entered(SimpleStateMachineCheckTrigger trigger) { }
        protected virtual void OnState2Entered(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState2Exited(SimpleStateMachineCheckTrigger trigger) {}
        protected virtual void OnState2Exited(SimpleStateMachineTrigger trigger) { }
        protected virtual void OnState2Exited(SimpleStateMachineContinueTrigger trigger) { }

        protected virtual void OnState3Entered(SimpleStateMachineTrigger trigger) { }


        protected virtual void OnState3Entered(SimpleStateMachineContinueTrigger trigger) { }

        protected virtual void OnState3Exited(SimpleStateMachineContinueTrigger trigger) { }
        protected virtual void OnState3Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState4Entered(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState4Exited(SimpleStateMachineTrigger trigger) { }

        protected virtual void OnState4Entered(SimpleStateMachineContinueTrigger trigger) { }

    }
}
