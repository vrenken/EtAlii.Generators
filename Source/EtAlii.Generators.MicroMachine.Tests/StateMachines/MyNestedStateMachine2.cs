// namespace EtAlii.Generators.MicroMachine.Tests
// {
//     using System;
//     using System.Threading.Tasks;
//
//     public class MyNestedStateMachine2 : MyNestedStateMachine2Base
//     {
//         protected override void OnState1Entered() => Console.WriteLine("State 1 entered");
//         protected override void OnState1EnteredFromStartTrigger(string name)
//         {
//             Console.WriteLine($"Name: {name}");
//             Continue();
//         }
//
//         protected override void OnState1Exited() => Console.WriteLine("State 1 exited");
//
//         protected override void OnState2Entered() => Console.WriteLine("State 2 entered");
//
//         protected override void OnState2EnteredFromContinueTrigger()
//         {
//             Console.WriteLine("Inside State2");
//             Continue();
//         }
//
//         protected override void OnState2Exited() => Console.WriteLine("State 2 exited");
//
//         protected override void OnState3Entered() => Console.WriteLine("State 3 entered");
//         protected override Task OnState3Exited() => Task.Run(() => Console.WriteLine("State 3 exited"));
//
//         protected override Task OnState4Entered() => Task.Run(() => Console.WriteLine("State 4 entered"));
//
//         protected override void OnState4Exited() => Console.WriteLine("State 4 exited");
//
//         protected override void OnState2InternalCheckTrigger(string name) => Console.WriteLine($"Name: {name}");
//
//         protected override void OnState3EnteredFromContinueTrigger() => Console.WriteLine("State 3 entered from continue trigger");
//
//         protected override Task OnState4EnteredFromContinueTrigger() => Task.Run(() => Console.WriteLine("State 4 entered from continue trigger"));
//
//         protected override void OnSubState1Entered() => Console.WriteLine("SubState 1 entered");
//
//         protected override void OnSubState1Exited() => Console.WriteLine("SubState 1 exited");
//
//         protected override void OnSubState1EnteredFromStartState2Trigger() => Console.WriteLine("SubState 1 entered from StartState 2 trigger");
//
//         protected override void OnSubState2Entered() => Console.WriteLine("SubState 2 entered");
//
//         protected override void OnSubState2Exited() => Console.WriteLine("SubState 2 exited");
//
//         protected override void OnSubState2EnteredFromContinueTrigger() => Console.WriteLine("SubState 2 entered from continue trigger");
//     }
// }
