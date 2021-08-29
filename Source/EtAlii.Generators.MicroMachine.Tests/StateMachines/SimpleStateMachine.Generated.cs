// namespace EtAlii.Generators.MicroMachine.Tests
// {
//     using System;
//     using System.Collections.Generic;
//     using Xunit;
//
//     public abstract class StateMachineTrigger
//     {
//
//     }
//
//     public abstract class Trigger : StateMachineTrigger
//     {
//
//     }
//
//     public class StartTrigger : Trigger
//     {
//
//     }
//     public class ContinueTrigger : Trigger
//     {
//
//     }
//     public class CheckTrigger : Trigger
//     {
//
//     }
//
//     public abstract class SimpleStateMachineBase
//     {
//         public enum State
//         {
//             Undefined,
//             State1,
//             State2,
//             State3,
//             State4,
//         }
//
//         private State _state;
//         private readonly Queue<Action> _transactions = new Queue<Action>();
//         private bool _isProcessingQueueUsingAsync;
//
//         private void RunOrQueueTransition(Action transition)
//         {
//             var deQueue = _transactions.Count > 0;
//             _transactions.Enqueue(transition);
//             if (deQueue)
//             {
//                 while(_transactions.TryDequeue(out var queuedTransaction))
//                 {
//                     queuedTransaction();
//                 }
//             }
//         }
//
//         private void StartTransition()
//         {
//             switch (_state)
//             {
//                 case State.Undefined:
//                     _state = State.State1;
//                     Trigger trigger = new StartTrigger();
//                     OnState1Entered(trigger);
//                     OnState1Entered((StartTrigger)trigger);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Start is not supported in state {_state}");
//             }
//         }
//
//         public void Start() => RunOrQueueTransition(StartTransition);
//
//         private void ContinueTransition()
//         {
//             switch (_state)
//             {
//                 case State.State1:
//                     _state = State.State2;
//                     var state1Trigger = new ContinueTrigger();
//                     OnState1Exited(state1Trigger);
//                     OnState1Exited((Trigger)state1Trigger);
//                     OnState2Entered((Trigger)state1Trigger);
//                     OnState2Entered(state1Trigger);
//                     break;
//                 case State.State2:
//                     _state = State.State3;
//                     var state2Trigger = new ContinueTrigger();
//                     OnState2Exited(state2Trigger);
//                     OnState2Exited((Trigger)state2Trigger);
//                     OnState3Entered((Trigger)state2Trigger);
//                     OnState3Entered(state2Trigger);
//                     break;
//                 case State.State3:
//                     _state = State.State4;
//                     var state3Trigger = new ContinueTrigger();
//                     OnState3Exited(state3Trigger);
//                     OnState3Exited((Trigger)state3Trigger);
//                     OnState4Entered((Trigger)state3Trigger);
//                     OnState4Entered(state3Trigger);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Continue is not supported in state {_state}");
//             }
//         }
//         public void Continue() => RunOrQueueTransition(ContinueTransition);
//
//         private void CheckTransition()
//         {
//             switch (_state)
//             {
//                 case State.State2:
//                     var state2Trigger = new CheckTrigger();
//                     OnState2Exited(state2Trigger);
//                     OnState2Exited((Trigger)state2Trigger);
//                     OnState2Entered((Trigger)state2Trigger);
//                     OnState2Entered(state2Trigger);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Check is not supported in state {_state}");
//             }
//         }
//
//         public void Check() => RunOrQueueTransition(CheckTransition);
//
//         protected virtual void OnState1Entered(Trigger trigger) { }
//         protected virtual void OnState1Entered(StartTrigger trigger) { }
//
//         protected virtual void OnState1Exited(ContinueTrigger trigger) { }
//         protected virtual void OnState1Exited(Trigger trigger) { }
//
//         protected virtual void OnState2Entered(ContinueTrigger trigger) { }
//         protected virtual void OnState2Entered(CheckTrigger trigger) { }
//         protected virtual void OnState2Entered(Trigger trigger) { }
//
//         protected virtual void OnState2Exited(CheckTrigger trigger) {}
//         protected virtual void OnState2Exited(Trigger trigger) { }
//         protected virtual void OnState2Exited(ContinueTrigger trigger) { }
//
//         protected virtual void OnState3Entered(Trigger trigger) { }
//
//
//         protected virtual void OnState3Entered(ContinueTrigger trigger) { }
//
//         protected virtual void OnState3Exited(ContinueTrigger trigger) { }
//         protected virtual void OnState3Exited(Trigger trigger) { }
//
//         protected virtual void OnState4Entered(Trigger trigger) { }
//
//         protected virtual void OnState4Exited(Trigger trigger) { }
//
//         protected virtual void OnState4Entered(ContinueTrigger trigger) { }
//
//     }
// }
