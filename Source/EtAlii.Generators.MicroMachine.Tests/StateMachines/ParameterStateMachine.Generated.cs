// // Remark: this file was auto-generated based on 'ParameterStateMachine.puml'.
// // Any changes will be overwritten the next time the file is generated.
//
// namespace EtAlii.Generators.MicroMachine.Tests
// {
//     using System;
//     using System.Threading.Tasks;
//     using System.Collections.Generic;
//     using EtAlii.Generators.MicroMachine.Tests.Nested;
//
//     /// <summary>
//     /// This is the abstract class for the state machine as defined in 'ParameterStateMachine.puml'.
//     /// Add another partial for the class and implement the transition methods to define the necessary business behavior.
//     /// The transitions can then be triggered by calling the corresponding trigger methods.
//     /// </summary>
//     public abstract class ParameterStateMachineBase
//     {
//         protected ParameterStateMachineBase.State _state;
//         private bool _queueTransitions;
//         private readonly Queue<Transition> _transactions = new Queue<Transition>();
//
//         private void RunOrQueueTransition(Transition transition)
//         {
//             if(_queueTransitions)
//             {
//                 _transactions.Enqueue(transition);
//             }
//             else
//             {
//                 _queueTransitions = true;
//                 _transactions.Enqueue(transition);
//                 while(_transactions.TryDequeue(out var queuedTransaction))
//                 {
//                     ((SyncTransition)queuedTransaction).Handler();
//                 }
//                 _queueTransitions = false;
//             }
//         }
//
//         private async Task RunOrQueueTransitionAsync(AsyncTransition transition)
//         {
//             if(_queueTransitions)
//             {
//                 _transactions.Enqueue(transition);
//             }
//             else
//             {
//                 _queueTransitions = true;
//                 _transactions.Enqueue(transition);
//                 while(_transactions.TryDequeue(out var queuedTransaction))
//                 {
//                     switch(queuedTransaction)
//                     {
//                         case SyncTransition syncTransition: syncTransition.Handler(); break;
//                         case AsyncTransition asyncTransition: await asyncTransition.Handler().ConfigureAwait(false); break;
//                         default: throw new InvalidOperationException("This will never happen");
//                     }
//                 }
//                 _queueTransitions = false;
//             }
//         }
//
//         // The methods below can be each called to fire a specific trigger
//         // and cause the state machine to transition to another state.
//
//         private void Activate1Transition()
//         {
//             switch (_state)
//             {
//                 case State.State1:
//                     _state = State.NextState1;
//                     Trigger triggerState1 = new Activate1Trigger();
//                     OnState1Exited((Activate1Trigger)triggerState1);
//                     OnState1Exited(triggerState1);
//                     OnNextState1Entered(triggerState1);
//                     OnNextState1Entered((Activate1Trigger)triggerState1);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Activate1 is not supported in state {_state}");
//             }
//         }
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// State1 --&gt; NextState1 : Activate1<br/>
//         /// </summary>
//         public void Activate1(string title, int count) => RunOrQueueTransition(new SyncTransition(Activate1Transition));// <string, int>(_activate1WithTitleAndCountTrigger, title, count);
//
//         private void ContinueTransition(string string0, int int1, float float2)
//         {
//             switch (_state)
//             {
//                 case State.State2:
//                     _state = State.NextState2;
//                     Trigger triggerState2 = new ContinueTrigger();
//                     OnState2Exited((ContinueTrigger)triggerState2);
//                     OnState2Exited(triggerState2);
//                     OnNextState2Entered(triggerState2);
//                     OnNextState2Entered((ContinueTrigger)triggerState2);
//                     break;
//                 case State.State3:
//                     _state = State.NextState3;
//                     Trigger triggerState3 = new ContinueTrigger();
//                     OnState3Exited((ContinueTrigger)triggerState3);
//                     OnState3Exited(triggerState3);
//                     OnNextState3Entered(triggerState3);
//                     OnNextState3Entered((ContinueTrigger)triggerState3);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Continue is not supported in state {_state}");
//             }
//         }
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// State2 --&gt; NextState2 : Continue<br/>
//         /// </summary>
//         public void Continue(string @string0, int @int1, float @float2) => RunOrQueueTransition(new SyncTransition(() => ContinueTransition(string0, int1, float2)));// <string, int, float>(_continueWithstringAndintAndfloatTrigger, @string0, @int1, @float2);
//
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// State3 --&gt; NextState3 : Continue<br/>
//         /// </summary>
//         public void Continue(Customer customer, Project project) => RunOrQueueTransition(new SyncTransition(ContinueTransition));// <Customer, Project>(_continueWithCustomerAndProjectTrigger, customer, project);
//
//         private void Continue1Transition()
//         {
//             switch (_state)
//             {
//                 case State._Begin:
//                     _state = State.State1;
//                     Trigger trigger_Begin = new Continue1Trigger();
//                     On_BeginExited((Continue1Trigger)trigger_Begin);
//                     On_BeginExited(trigger_Begin);
//                     OnState1Entered(trigger_Begin);
//                     OnState1Entered((Continue1Trigger)trigger_Begin);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Continue1 is not supported in state {_state}");
//             }
//         }
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// _Begin --&gt; State1 : Continue1<br/>
//         /// </summary>
//         public void Continue1() => RunOrQueueTransition(new SyncTransition(Continue1Transition));// (Trigger.Continue1);
//
//         private void Continue2Transition()
//         {
//             switch (_state)
//             {
//                 case State._Begin:
//                     _state = State.State2;
//                     Trigger trigger_Begin = new Continue2Trigger();
//                     On_BeginExited((Continue2Trigger)trigger_Begin);
//                     On_BeginExited(trigger_Begin);
//                     OnState2Entered(trigger_Begin);
//                     OnState2Entered((Continue2Trigger)trigger_Begin);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Continue2 is not supported in state {_state}");
//             }
//         }
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// _Begin --&gt; State2 : Continue2<br/>
//         /// </summary>
//         public void Continue2() => RunOrQueueTransition(new SyncTransition(Continue2Transition));// (Trigger.Continue2);
//
//         private void Continue3Transition(string title, int count)
//         {
//             switch (_state)
//             {
//                 case State._Begin:
//                     _state = State.State3;
//                     Trigger trigger_Begin = new Continue3Trigger();
//                     On_BeginExited((Continue3Trigger)trigger_Begin);
//                     On_BeginExited(trigger_Begin);
//                     OnState3Entered(trigger_Begin);
//                     OnState3Entered((Continue3Trigger)trigger_Begin);
//                     break;
//                 default:
//                     throw new NotSupportedException($"Trigger Continue3 is not supported in state {_state}");
//             }
//         }
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// _Begin --&gt; State3 : Continue3<br/>
//         /// </summary>
//         public void Continue3(string title, int count) => RunOrQueueTransition(new SyncTransition(() => Continue3Transition(title, count)));// (Trigger.Continue3);
//
//
//         // The classes below represent the transitions as used by the state machine.
//
//         protected abstract class Transition
//         {
//         }
//
//         protected sealed class AsyncTransition : Transition
//         {
//             public Func<Task> Handler {get; init;}
//
//             public AsyncTransition(Func<Task> handler) => Handler = handler;
//         }
//
//         protected sealed class SyncTransition : Transition
//         {
//             public Action Handler {get; init;}
//
//             public SyncTransition(Action handler) => Handler = handler;
//         }
//
//
//         // The classes below represent the triggers as used by the methods.
//
//         protected class Trigger
//         {
//         }
//
//         protected class Activate1Trigger : Trigger
//         {
//         }
//
//         protected class ContinueTrigger : Trigger
//         {
//         }
//
//         protected class Continue1Trigger : Trigger
//         {
//         }
//
//         protected class Continue2Trigger : Trigger
//         {
//         }
//
//         protected class Continue3Trigger : Trigger
//         {
//         }
//
//
//         // Of course each state machine needs a set of states.
//         protected enum State
//         {
//             _Begin,
//             NextState1,
//             NextState2,
//             NextState3,
//             State1,
//             State2,
//             State3,
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the '_Begin' state.
//         /// </summary>
//         protected virtual void On_BeginEntered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the '_Begin' state.
//         /// </summary>
//         protected virtual void On_BeginExited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the '_Begin' state by the 'Continue1' trigger.
//         /// </summary>
//         protected virtual void On_BeginExited(Continue1Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the '_Begin' state by the 'Continue2' trigger.
//         /// </summary>
//         protected virtual void On_BeginExited(Continue2Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the '_Begin' state by the 'Continue3' trigger.
//         /// </summary>
//         protected virtual void On_BeginExited(Continue3Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'NextState1' state.
//         /// </summary>
//         protected virtual void OnNextState1Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'NextState1' state.
//         /// </summary>
//         protected virtual void OnNextState1Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'NextState1' state by the 'Activate1' trigger.
//         /// </summary>
//         protected virtual void OnNextState1Entered(Activate1Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'NextState2' state.
//         /// </summary>
//         protected virtual void OnNextState2Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'NextState2' state.
//         /// </summary>
//         protected virtual void OnNextState2Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'NextState2' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnNextState2Entered(ContinueTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'NextState3' state.
//         /// </summary>
//         protected virtual void OnNextState3Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'NextState3' state.
//         /// </summary>
//         protected virtual void OnNextState3Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'NextState3' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnNextState3Entered(ContinueTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State1' state.
//         /// </summary>
//         protected virtual void OnState1Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State1' state.
//         /// </summary>
//         protected virtual void OnState1Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State1' state by the 'Continue1' trigger.
//         /// </summary>
//         protected virtual void OnState1Entered(Continue1Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State1' state by the 'Activate1' trigger.
//         /// </summary>
//         protected virtual void OnState1Exited(Activate1Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State2' state.
//         /// </summary>
//         protected virtual void OnState2Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State2' state.
//         /// </summary>
//         protected virtual void OnState2Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State2' state by the 'Continue2' trigger.
//         /// </summary>
//         protected virtual void OnState2Entered(Continue2Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State2' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState2Exited(ContinueTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State3' state.
//         /// </summary>
//         protected virtual void OnState3Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State3' state.
//         /// </summary>
//         protected virtual void OnState3Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State3' state by the 'Continue3' trigger.
//         /// </summary>
//         protected virtual void OnState3Entered(Continue3Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State3' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState3Exited(ContinueTrigger trigger)
//         {
//         }
//     }
// }
