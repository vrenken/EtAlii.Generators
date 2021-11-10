namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class ToDifferentStateMethodChainBuilder
    {
        private readonly ILogger _log = Log.ForContext<ToDifferentStateMethodChainBuilder>();

        private readonly StateFragmentHelper _stateFragmentHelper;

        public ToDifferentStateMethodChainBuilder(StateFragmentHelper stateFragmentHelper)
        {
            _stateFragmentHelper = stateFragmentHelper;
        }

        public MethodChain[] Build(StateMachine stateMachine, State fromState, State toState)
        {
            var result = new List<MethodChain>();

            // First let's write the transition for the from state to the to state.
            var methodChain = BuildForOneSingleSourceState(stateMachine, fromState, toState);
            result.Add(methodChain);

            foreach (var childState in fromState.AllChildren)
            {
                var subMethodChain = BuildForOneSingleSourceState(stateMachine, childState, toState);
                result.Add(subMethodChain);
            }

            return result.ToArray();
        }

        public MethodChain BuildForOneSingleSourceState(StateMachine stateMachine, State fromState, State toState)
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
            var fromParents = _stateFragmentHelper.GetAllSuperStates(stateMachine, fromState.Name);

            var parentStates = fromParents.Any()
                ? string.Join(", ", fromParents.Select(s => s.Name).ToArray())
                : "[NONE]";
            _log.Debug("Acquired all superstates for {FromState}: {ParentStates}", fromState, parentStates);

            var toParents = _stateFragmentHelper.GetAllSuperStates(stateMachine, toState.Name);
            parentStates = toParents.Any()
                ? string.Join(", ", toParents.Select(s => s.Name).ToArray())
                : "[NONE]";
            _log.Debug("Acquired all superstates for {ToState}: {ParentStates}", toState, parentStates);

            var toIsChildOfFrom = toParents.Any(toParent => toParent.Name == fromState.Name);
            var fromIsChildOfTo = fromParents.Any(fromParent => fromParent.Name == toState.Name);

            var sharedParent = GetSharedParent(toParents, fromParents);

            _log.Debug("Determined structure: {ToIsChildOfFrom} and {FromIsChildOfTo}", toIsChildOfFrom, fromIsChildOfTo);

            if (!toIsChildOfFrom)
            {
                _log.Debug("{ToState} is a child of {FromState}", toState, fromState);

                // 2. - Pick the state itself.
                exitCalls.Add(new MethodCall(fromState, false));
                // 3. - Pick any state except beyond the shared superstate.
                foreach (var fromParent in fromParents)
                {
                    var shouldWriteExitState = fromParent.Name != toState.Name && fromParent.Name != sharedParent?.Name;

                    if (!shouldWriteExitState)
                    {
                        break;
                    }
                    var fromParentState = stateMachine.SequentialStates.Single(s => s.Name == fromParent.Name);
                    exitCalls.Add(new MethodCall(fromParentState, true));
                }
            }

            if (!fromIsChildOfTo)
            {
                _log.Debug("{FromState} is a child of {ToState}", fromState, toState);

                // 4. - Pick the state itself.
                entryCalls.Add(new MethodCall(toState, false));
                // 5. - Pick any state except the shared superstate.
                foreach (var toParent in toParents)
                {
                    var shouldWriteEntryState = toParent.Name != fromState.Name && toParent.Name != sharedParent?.Name;

                    if (!shouldWriteEntryState)
                    {
                        break;
                    }

                    var toParentState = stateMachine.SequentialStates.Single(s => s.Name == toParent.Name);
                    entryCalls.Add(new MethodCall(toParentState, true));
                }
                // 6. - Reverse the order.
                entryCalls.Reverse();
            }

            return new MethodChain { From = fromState, To = toState, ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() };
        }

        private SuperState GetSharedParent(SuperState[] toParents, SuperState[] fromParents)
        {
            foreach (var toParent in toParents)
            {
                foreach (var fromParent in fromParents)
                {
                    if (toParent.Name == fromParent.Name)
                    {
                        return toParent;
                    }
                }
            }
            return null;
        }
    }
}
