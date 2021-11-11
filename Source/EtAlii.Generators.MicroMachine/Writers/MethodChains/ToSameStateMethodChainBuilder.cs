// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using EtAlii.Generators.PlantUml;

    public class ToSameStateMethodChainBuilder
    {
        private readonly ToDifferentStateMethodChainBuilder _toDifferentStateMethodChainBuilder;

        public ToSameStateMethodChainBuilder(ToDifferentStateMethodChainBuilder toDifferentStateMethodChainBuilder)
        {
            _toDifferentStateMethodChainBuilder = toDifferentStateMethodChainBuilder;
        }

        public MethodChain[] Build(StateMachine stateMachine, State state, bool isAsync)
        {
            var result = new List<MethodChain>();

            var methodChain = BuildForSameState(state, isAsync);
            result.Add(methodChain);

            foreach (var childState in state.AllChildren)
            {
                var subMethodChain = _toDifferentStateMethodChainBuilder.BuildForOneSingleSourceState(stateMachine, childState, state, isAsync);
                result.Add(subMethodChain);
            }

            return result.ToArray();
        }

        private MethodChain BuildForSameState(State state, bool isAsync)
        {
            var exitCalls = new List<MethodCall>();
            var entryCalls = new List<MethodCall>();

            // Pick the state itself.
            // TODO: This should be replicated for all substate chains.
            exitCalls.Add(new MethodCall(state, false, isAsync));

            // Pick the state itself.
            entryCalls.Add(new MethodCall(state, false, isAsync));

            return new MethodChain { From = state, To = state, ExitCalls = exitCalls.ToArray(), EntryCalls = entryCalls.ToArray() };
        }
    }
}
