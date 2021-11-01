// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameterInPartialMethod
namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public partial class SimplePartialStateMachine
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        partial void OnState1Entered(Trigger trigger, State1Choices choices) => LogTransition(typeof(Trigger));
        partial void OnState1Entered(StartTrigger trigger, State1Choices choices) => LogTransition(typeof(StartTrigger));
        partial void OnState1Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        partial void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnState2Entered(Trigger trigger, State2Choices choices) => LogTransition(typeof(Trigger));
        partial void OnState2Entered(ContinueTrigger trigger, State2Choices choices) => LogTransition(typeof(ContinueTrigger));
        partial void OnState2Entered(CheckTrigger trigger, State2Choices choices) => LogTransition(typeof(CheckTrigger));
        partial void OnState2Exited(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));
        partial void OnState2Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        partial void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnState3Entered(Trigger trigger, State3Choices choices) => LogTransition(typeof(Trigger));
        partial void OnState3Entered(ContinueTrigger trigger, State3Choices choices)
        {
            LogTransition(typeof(ContinueTrigger));
            Continue();
        }
        partial void OnState3Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        partial void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnState4Entered(Trigger trigger, State4Choices choices) => LogTransition(typeof(Trigger));
        partial void OnState4Entered(ContinueTrigger trigger, State4Choices choices) => LogTransition(typeof(ContinueTrigger));
        partial void OnState4Exited(Trigger trigger) => LogTransition(typeof(Trigger));
    }
}
