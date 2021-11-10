namespace EtAlii.Generators.MicroMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class MethodChainBuilder
    {
        private readonly ILogger _log = Log.ForContext<MethodChainBuilder>();

        private readonly ToDifferentStateMethodChainBuilder _toDifferentStateMethodChainBuilder;
        private readonly ToSameStateMethodChainBuilder _toSameStateMethodChainBuilder;
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly IStateMachineLifetime _lifetime;

        public MethodChainBuilder(
            ToDifferentStateMethodChainBuilder toDifferentStateMethodChainBuilder,
            ToSameStateMethodChainBuilder toSameStateMethodChainBuilder,
            StateFragmentHelper stateFragmentHelper,
            IStateMachineLifetime lifetime)
        {
            _toDifferentStateMethodChainBuilder = toDifferentStateMethodChainBuilder;
            _toSameStateMethodChainBuilder = toSameStateMethodChainBuilder;
            _stateFragmentHelper = stateFragmentHelper;
            _lifetime = lifetime;
        }

        private readonly Dictionary<Transition, MethodChain[]> _methodChains = new();

        public MethodChain[] Build(StateMachine stateMachine, Transition transition)
        {
            if (!_methodChains.TryGetValue(transition, out var methodChains))
            {
                var parentSuperState = _stateFragmentHelper.GetSuperState(stateMachine, transition.To);
                var fromStateName = parentSuperState != null && transition.From == _lifetime.BeginStateName
                    ? parentSuperState.Name
                    : transition.From;
                var trigger = transition.Trigger;
                var fromState = stateMachine.SequentialStates.Single(s => s.Name == fromStateName);
                var toState = stateMachine.SequentialStates.Single(s => s.Name == transition.To);

                _log.Information("Building method chain for transition from {FromState} by {Trigger} to {ToState}", fromState, trigger, toState);

                if (fromState == toState)
                {
                    return _toSameStateMethodChainBuilder.Build(stateMachine, fromState);
                }
                _methodChains[transition] = methodChains = _toDifferentStateMethodChainBuilder.Build(stateMachine, fromState, toState);
            }

            return methodChains;
        }
    }
}
