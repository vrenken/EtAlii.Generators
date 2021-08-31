namespace EtAlii.Generators.MicroMachine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;

    public class MethodChain
    {
        public MethodCall[] ExitCalls { get; private set; }
        public MethodCall[] EntryCalls { get; private set; }


        public static MethodChain Create(WriteContext<StateMachine> context, string fromState, string trigger, string toState)
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
            var fromParents = StateFragment.GetAllSuperStates(context.Instance, fromState);
            var toParents = StateFragment.GetAllSuperStates(context.Instance, toState);
            var sharedParent = Array.Find(fromParents, fromParent => toParents.Any(toParent => SuperStatesAreEqual(fromParent, toParent)));
            var toIsChildOfFrom = toParents.Any(toParent => toParent.Name == fromState);
            var fromIsChildOfTo = fromParents.Any(fromParent => fromParent.Name == toState);

            // 2. - Pick the state itself.
            if (!toIsChildOfFrom)
            {
                exitCalls.Add(new MethodCall(fromState, false, false));
                foreach (var fromParent in fromParents)
                {
                    //var shouldWriteExitState = false;
                    //shouldWriteExitState |= sharedParent != null && SuperStatesAreEqual(fromParent, sharedParent);
                    var shouldWriteExitState = fromParent.Name != toState;

                    if (!shouldWriteExitState)
                    {
                        break;
                    }
                    exitCalls.Add(new MethodCall(fromParent.Name, true, false));
                }
            }

            // 4. - Pick the state itself.
            if (!fromIsChildOfTo)
            {
                entryCalls.Add(new MethodCall(toState, false, false));
                // 5. - Pick any state except the shared superstate.
                foreach (var toParent in toParents)
                {
                    //var shouldWriteEntryState = false;
                    //shouldWriteEntryState |= sharedParent != null && SuperStatesAreEqual(toParent, sharedParent);
                    var shouldWriteEntryState = toParent.Name != fromState;

                    if (!shouldWriteEntryState)
                    {
                        break;
                    }

                    entryCalls.Add(new MethodCall(toParent.Name, true, false));
                }
                // 6. - Reverse the order.
                entryCalls.Reverse();
            }

            return new MethodChain { ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() };
        }

        private static bool SuperStatesAreEqual(SuperState s1, SuperState s2) => s1.Source.Line == s2.Source.Line && s1.Source.Column == s2.Source.Column;
    }
}
