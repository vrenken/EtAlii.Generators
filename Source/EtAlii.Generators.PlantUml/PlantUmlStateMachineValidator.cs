// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.PlantUml
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// The central class responsible of validating PlantUML specific state machine requirements
    /// and express them using Roslyn Diagnostic instances.
    /// </summary>
    public class PlantUmlStateMachineValidator : IValidator<StateMachine>
    {
        private readonly IStateMachineLifetime _lifetime;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public PlantUmlStateMachineValidator(IStateMachineLifetime lifetime, StateFragmentHelper stateFragmentHelper)
        {
            _lifetime = lifetime;
            _stateFragmentHelper = stateFragmentHelper;
        }

        public void Validate(StateMachine instance, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            CheckForStartStates(instance, fullPathToFile, diagnostics);

            CheckForDuplicateTriggers(instance, fullPathToFile, diagnostics);

            CheckForUnnamedParameters(instance, fullPathToFile, diagnostics);

            CheckForUnnamedTriggers(instance, fullPathToFile, diagnostics);

            CheckSubstatesEntryTransition(instance, fullPathToFile, diagnostics);
        }

        private void CheckForDuplicateTriggers(StateMachine stateMachine, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            var transitionsWithDuplicateTriggers = _stateFragmentHelper
                .GetAllTransitions(stateMachine.StateFragments)
                .GroupBy(t => t.From)
                .SelectMany(sg =>
                {
                    return sg
                        .GroupBy(tg => tg.Trigger)
                        .Select(tg => new {State = sg.Key, Trigger = tg.Key, tg.First().Source, Count = tg.Count()})
                        .ToArray();
                })
                .Where(r => r.Count > 1)
                .ToArray();

            foreach (var transitionWithDuplicateTriggers in transitionsWithDuplicateTriggers)
            {
                var source = transitionWithDuplicateTriggers.Source;
                var location = source.ToLocation(fullPathToFile);
                var diagnostic = Diagnostic.Create(GeneratorRule.DuplicateTriggers, location, transitionWithDuplicateTriggers.Trigger, transitionWithDuplicateTriggers.Count, transitionWithDuplicateTriggers.State);

                diagnostics.Add(diagnostic);
            }
        }

        private void CheckSubstatesEntryTransition(StateMachine stateMachine, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            var superStates = _stateFragmentHelper.GetAllSuperStates(stateMachine.StateFragments);
            foreach (var superState in superStates)
            {
                var allSubstates = _stateFragmentHelper.GetAllSubStates(superState);
                var allTransitions = _stateFragmentHelper.GetAllTransitions(stateMachine.StateFragments);

                var directTransitionsToSubState = allTransitions
                    .Where(t => t.From != superState.Name && allSubstates.Contains(t.To) && !allSubstates.Contains(t.From))
                    .ToArray();

                var transitionsToSuperState = allTransitions
                    .Where(t => t.To == superState.Name)
                    .ToArray();

                var superStateStartTransitions = superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From != t.To && t.From == _lifetime.BeginStateName)
                    .ToArray();

                var namedSuperStateStartTransitions = superStateStartTransitions
                    .Where(t => t.HasConcreteTriggerName)
                    .ToArray();

                var unnamedSuperStateStartTransitions = superStateStartTransitions
                    .Where(t => !t.HasConcreteTriggerName)
                    .ToArray();

                if (!transitionsToSuperState.Any() && !directTransitionsToSubState.Any())
                {
                    // The superstate is not being used. Let's not throw a warning/error for now.
                }

                // UNNAMED+ DIRECT+ => No can do
                if (unnamedSuperStateStartTransitions.Any() && directTransitionsToSubState.Any())
                {
                    // As we cannot guarantee an adequate sequential order of execution we don't support both unnamed start transitions and direct substate transitions.
                    var location = superState.Source.ToLocation(fullPathToFile);
                    var diagnostic = Diagnostic.Create(GeneratorRule.SuperstateHasBothUnnamedAndDirectTransitionsDefined, location, superState.Source.Text);
                    diagnostics.Add(diagnostic);
                }

                // ~NAMED UNNAMED+ ~DIRECT
                if (!namedSuperStateStartTransitions.Any() && unnamedSuperStateStartTransitions.Any() && !directTransitionsToSubState.Any() && unnamedSuperStateStartTransitions.Length > 1)
                {
                    var location = superState.Source.ToLocation(fullPathToFile);
                    var diagnostic = Diagnostic.Create(GeneratorRule.SuperstateHasMultipleUnnamedStartTransitionsDefined, location, superState.Source.Text);
                    diagnostics.Add(diagnostic);
                }
            }
        }

        private void CheckForUnnamedTriggers(StateMachine stateMachine, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            var allTransitions = _stateFragmentHelper.GetAllTransitions(stateMachine.StateFragments);
            var transitionsWithUnnamedTrigger = allTransitions
                .Where(t => !t.HasConcreteTriggerName)
                .ToArray();

            foreach (var transitionWithUnnamedTrigger in transitionsWithUnnamedTrigger)
            {
                var location = transitionWithUnnamedTrigger.Source.ToLocation(fullPathToFile);
                var diagnostic = Diagnostic.Create(GeneratorRule.UnnamedTrigger, location, transitionWithUnnamedTrigger.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        private void CheckForUnnamedParameters(StateMachine stateMachine, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            var allTransitions = _stateFragmentHelper.GetAllTransitions(stateMachine.StateFragments);
            var unnamedParameters = allTransitions
                .Where(t => t.Parameters.Any(p => !p.HasName))
                .Select(t => t.Parameters.First(p => !p.HasName))
                .ToArray();

            foreach (var unnamedParameter in unnamedParameters)
            {
                var location = unnamedParameter.Source.ToLocation(fullPathToFile);
                var diagnostic = Diagnostic.Create(GeneratorRule.UnnamedParameter, location, unnamedParameter.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        private void CheckForStartStates(StateMachine stateMachine, string fullPathToFile, List<Diagnostic> diagnostics)
        {
            var allTransitions = _stateFragmentHelper.GetAllTransitions(stateMachine.StateFragments);
            var startStates = allTransitions
                .Where(t => t.From == _lifetime.BeginStateName)
                .ToArray();
            if (startStates.Length == 0)
            {
                var location = Location.Create(fullPathToFile, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(GeneratorRule.NoStartStatesDefined, location);
                diagnostics.Add(diagnostic);
            }
        }
    }
}
