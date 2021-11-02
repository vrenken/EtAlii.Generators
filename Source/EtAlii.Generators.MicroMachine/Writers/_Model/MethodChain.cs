namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class MethodChain
    {
        public MethodCall[] ExitCalls { get; private set; }
        public MethodCall[] EntryCalls { get; private set; }

        public static MethodChain Create(WriteContext<StateMachine> context, StateFragmentHelper stateFragmentHelper, string fromState, string trigger, string toState)
        {
            var exitCalls = new List<MethodCall>();
            var entryCalls = new List<MethodCall>();

            // 1. - Find the biggest shared superstate / or the complete state machine.
            // Exit:
            // 2. - Pick the state itself.
            // 3. - Pick any state except beyond the shared superstate.
            // Entry:
            // 4. - Pick the state itself.
            // 5. - Pick any state except beyond the shared superstate.
            // 6. - Reverse the order.

            // 1. - Find the biggest shared superstate / or the complete state machine.
            var fromParents = stateFragmentHelper.GetAllSuperStates(context.Instance, fromState);
            var toParents = stateFragmentHelper.GetAllSuperStates(context.Instance, toState);
            var toIsChildOfFrom = toParents.Any(toParent => toParent.Name == fromState);
            var fromIsChildOfTo = fromParents.Any(fromParent => fromParent.Name == toState);

            if (!toIsChildOfFrom)
            {
                // 2. - Pick the state itself.
                exitCalls.Add(new MethodCall(fromState, false));
                // 3. - Pick any state except beyond the shared superstate.
                foreach (var fromParent in fromParents)
                {
                    var shouldWriteExitState = fromParent.Name != toState;

                    if (!shouldWriteExitState)
                    {
                        break;
                    }
                    exitCalls.Add(new MethodCall(fromParent.Name, true));
                }
            }

            if (!fromIsChildOfTo)
            {
                // 4. - Pick the state itself.
                entryCalls.Add(new MethodCall(toState, false));
                // 5. - Pick any state except the shared superstate.
                foreach (var toParent in toParents)
                {
                    var shouldWriteEntryState = toParent.Name != fromState;

                    if (!shouldWriteEntryState)
                    {
                        break;
                    }

                    entryCalls.Add(new MethodCall(toParent.Name, true));
                }
                // 6. - Reverse the order.
                entryCalls.Reverse();
            }

            return new MethodChain { ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() };
        }
    }
}
