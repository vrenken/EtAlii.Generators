// // Remark: this file was auto-generated based on 'SimpleStateMachine.puml'.
// // Any changes will be overwritten the next time the file is generated.
//
// namespace EtAlii.Generators.MicroMachine.Tests
// {
//     using System;
//     using System.Threading.Tasks;
//     using System.Collections.Generic;
//
//     /// <summary>
//     /// This is the abstract class for the state machine as defined in 'SimpleStateMachine.puml'.
//     /// Add another partial for the class and implement the transition methods to define the necessary business behavior.
//     /// The transitions can then be triggered by calling the corresponding trigger methods.
//     /// </summary>
//     public abstract class SimpleStateMachineBase
//     {
//         protected SimpleStateMachineBase.State _state;
//         private readonly Queue<Action> _triggers = new Queue<Action>();
//
//         // The methods below can be each called to fire a specific trigger
//         // and cause the state machine to transition to another state.
//
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// State2 --&gt; State2 : Check<br/>
//         /// </summary>
//         public void Check() { }// _stateMachine.Fire(Trigger.Check);
//
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// State1 --&gt; State2 : Continue<br/>
//         /// State2 --&gt; State3 : Continue<br/>
//         /// State3 --&gt; State4 : Continue<br/>
//         /// </summary>
//         public void Continue() { }// _stateMachine.Fire(Trigger.Continue);
//
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// State2 --&gt; _End : Exit<br/>
//         /// </summary>
//         public void Exit() { }// _stateMachine.Fire(Trigger.Exit);
//
//         /// <summary>
//         /// Depending on the current state, call this method to trigger one of the sync transitions below:<br/>
//         /// _Begin --&gt; State1 : Start<br/>
//         /// </summary>
//         public void Start() { }// _stateMachine.Fire(Trigger.Start);
//
//
//         // The classes below represent the triggers as used by the methods.
//
//         protected class Trigger
//         {
//         }
//
//         protected class CheckTrigger : Trigger
//         {
//         }
//
//         protected class ContinueTrigger : Trigger
//         {
//         }
//
//         protected class ExitTrigger : Trigger
//         {
//         }
//
//         protected class StartTrigger : Trigger
//         {
//         }
//
//
//         // Of course each state machine needs a set of states.
//         protected enum State
//         {
//             _Begin,
//             _End,
//             State1,
//             State2,
//             State3,
//             State4,
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
//         /// Implement this method to handle the exit of the '_Begin' state by the 'Start' trigger.
//         /// </summary>
//         protected virtual void On_BeginExited(StartTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the '_End' state.
//         /// </summary>
//         protected virtual void On_EndEntered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the '_End' state.
//         /// </summary>
//         protected virtual void On_EndExited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the '_End' state by the 'Exit' trigger.
//         /// </summary>
//         protected virtual void On_EndEntered(ExitTrigger trigger)
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
//         /// Implement this method to handle the entry of the 'State1' state by the 'Start' trigger.
//         /// </summary>
//         protected virtual void OnState1Entered(StartTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State1' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState1Exited(ContinueTrigger trigger)
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
//         /// Implement this method to handle the entry of the 'State2' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState2Entered(ContinueTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State2' state by the 'Exit' trigger.
//         /// </summary>
//         protected virtual void OnState2Exited(ExitTrigger trigger)
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
//         /// Implement this method to handle the entry of the 'State2' state by the 'Check' trigger.
//         /// </summary>
//         protected virtual void OnState2Entered(CheckTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State2' state by the 'Check' trigger.
//         /// </summary>
//         protected virtual void OnState2Exited(CheckTrigger trigger)
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
//         /// Implement this method to handle the entry of the 'State3' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState3Entered(ContinueTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State3' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState3Exited(ContinueTrigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State4' state.
//         /// </summary>
//         protected virtual void OnState4Entered(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the exit of the 'State4' state.
//         /// </summary>
//         protected virtual void OnState4Exited(Trigger trigger)
//         {
//         }
//
//         /// <summary>
//         /// Implement this method to handle the entry of the 'State4' state by the 'Continue' trigger.
//         /// </summary>
//         protected virtual void OnState4Entered(ContinueTrigger trigger)
//         {
//         }
//
//     }
// }
