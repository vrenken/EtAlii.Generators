namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class MethodChainBuilder
    {
        private readonly ILogger _log = Log.ForContext<MethodChainBuilder>();

        public MethodChain[] Build(WriteContext<StateMachine> context, StateFragmentHelper stateFragmentHelper, string fromState, string trigger, string toState)
        {
            _log.Information("Building method chain for transition from {FromState} by {Trigger} to {ToState}", fromState, trigger, toState);

            if (fromState == toState)
            {
                return BuildChainToSameState(fromState);
            }
            return BuildChainToDifferentState(context, stateFragmentHelper, fromState, toState);
        }

        private MethodChain[] BuildChainToDifferentState(WriteContext<StateMachine> context, StateFragmentHelper stateFragmentHelper, string fromState, string toState)
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
            var parentStates = fromParents.Any()
                ? string.Join(", ", fromParents.Select(s => s.Name).ToArray())
                : "[NONE]";
            _log.Debug("Acquired all superstates for {FromState}: {ParentStates}", fromState, parentStates);

            var toParents = stateFragmentHelper.GetAllSuperStates(context.Instance, toState);
            parentStates = toParents.Any()
                ? string.Join(", ", toParents.Select(s => s.Name).ToArray())
                : "[NONE]";
            _log.Debug("Acquired all superstates for {ToState}: {ParentStates}", toState, parentStates);

            var toIsChildOfFrom = toParents.Any(toParent => toParent.Name == fromState);
            var fromIsChildOfTo = fromParents.Any(fromParent => fromParent.Name == toState);

            _log.Debug("Determined structure: {ToIsChildOfFrom} and {FromIsChildOfTo}", toIsChildOfFrom, fromIsChildOfTo);

            if (!toIsChildOfFrom)
            {
                _log.Debug("{ToState} is a child of {FromState}", toState, fromState);

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
                _log.Debug("{FromState} is a child of {ToState}", fromState, toState);

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

            return new [] { new MethodChain { ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() } };
        }

        private MethodChain[] BuildChainToSameState(string state)
        {
            var exitCalls = new List<MethodCall>();
            var entryCalls = new List<MethodCall>();

            // Pick the state itself.
            // TODO: This should be replicated for all substate chains.
            exitCalls.Add(new MethodCall(state, false));

            // Pick the state itself.
            entryCalls.Add(new MethodCall(state, false));

            return new [] { new MethodChain { ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() } };
        }
    }
}
