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

        private partial void OnState1Entered(Trigger trigger, State1Choices choices) => LogTransition(typeof(Trigger));
        private partial void OnState1Entered(StartTrigger trigger, State1Choices choices) => LogTransition(typeof(StartTrigger));
        private partial void OnState1Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        private partial void OnState1Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnState2Entered(Trigger trigger, State2Choices choices) => LogTransition(typeof(Trigger));
        private partial void OnState2Entered(ContinueTrigger trigger, State2Choices choices) => LogTransition(typeof(ContinueTrigger));
        private partial void OnState2Entered(CheckTrigger trigger, State2Choices choices) => LogTransition(typeof(CheckTrigger));
        private partial void OnState2Exited(CheckTrigger trigger) => LogTransition(typeof(CheckTrigger));
        private partial void OnState2Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        private partial void OnState2Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnState3Entered(Trigger trigger, State3Choices choices) => LogTransition(typeof(Trigger));
        private partial void OnState3Entered(ContinueTrigger trigger, State3Choices choices)
        {
            LogTransition(typeof(ContinueTrigger));
            Continue();
        }
        private partial void OnState3Exited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        private partial void OnState3Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnState4Entered(Trigger trigger, State4Choices choices) => LogTransition(typeof(Trigger));
        private partial void OnState4Entered(ContinueTrigger trigger, State4Choices choices) => LogTransition(typeof(ContinueTrigger));
        private partial void OnState4Exited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void On_IdleEntered(ExitTrigger trigger, _IdleChoices choices) { }

        private partial void On_IdleEntered(Trigger trigger, _IdleChoices choices) { }

        private partial void On_IdleExited(Trigger trigger) { }

        private partial void On_IdleExited(StartTrigger trigger) { }

        private partial void OnState2Exited(ExitTrigger trigger) { }
    }
}
