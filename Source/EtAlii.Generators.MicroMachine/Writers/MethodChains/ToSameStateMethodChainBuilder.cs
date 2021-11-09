// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using EtAlii.Generators.PlantUml;

    public class ToSameStateMethodChainBuilder
    {
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly ToDifferentStateMethodChainBuilder _toDifferentStateMethodChainBuilder;

        public ToSameStateMethodChainBuilder(StateFragmentHelper stateFragmentHelper, ToDifferentStateMethodChainBuilder toDifferentStateMethodChainBuilder)
        {
            _stateFragmentHelper = stateFragmentHelper;
            _toDifferentStateMethodChainBuilder = toDifferentStateMethodChainBuilder;
        }

        public MethodChain[] Build(StateMachine stateMachine, string state)
        {
            var result = new List<MethodChain>();

            var methodChain = BuildForSameState(state);
            result.Add(methodChain);

            var allSubFromStates = _stateFragmentHelper.GetAllSubStates(stateMachine, state);

            foreach (var subFromState in allSubFromStates)
            {
                var subMethodChain = _toDifferentStateMethodChainBuilder.BuildForOneSingleSourceState(stateMachine, subFromState, state);
                result.Add(subMethodChain);
            }

            return result.ToArray();
        }

        private MethodChain BuildForSameState(string state)
        {
            var exitCalls = new List<MethodCall>();
            var entryCalls = new List<MethodCall>();

            // Pick the state itself.
            // TODO: This should be replicated for all substate chains.
            exitCalls.Add(new MethodCall(state, false));

            // Pick the state itself.
            entryCalls.Add(new MethodCall(state, false));

            return new MethodChain { From = state, To = state, ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() };
        }
    }
}
