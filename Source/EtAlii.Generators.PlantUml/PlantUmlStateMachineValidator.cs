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

        public PlantUmlStateMachineValidator(IStateMachineLifetime lifetime)
        {
            _lifetime = lifetime;
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
            var transitionsWithDuplicateTriggers = stateMachine.AllTransitions
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
            foreach (var superState in stateMachine.AllSuperStates)
            {
                var state = stateMachine.SequentialStates.Single(s => s.Name == superState.Name);
                var directTransitionsToSubState = stateMachine.AllTransitions
                    .Where(
                        t =>
                            t.From != superState.Name &&
                            state.AllSubStates.Contains(t.To) &&
                            !state.AllSubStates.Contains(t.From) &&
                            t.To != _lifetime.BeginStateName &&
                            t.To != _lifetime.EndStateName)
                    .ToArray();

                var transitionsToSuperState = stateMachine.AllTransitions
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

                    var additionalSourceLocations = unnamedSuperStateStartTransitions
                        .Select(t => t.Source.ToLocation(fullPathToFile))
                        .Concat(namedSuperStateStartTransitions.Select(t => t.Source.ToLocation(fullPathToFile)))
                        .AsEnumerable();
                    var diagnostic = Diagnostic.Create(GeneratorRule.SuperstateHasBothUnnamedAndDirectTransitionsDefined, location, additionalSourceLocations, null, superState.Source.Text);
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
            var transitionsWithUnnamedTrigger = stateMachine.AllTransitions
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
            var unnamedParameters = stateMachine.AllTransitions
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
            var startStates = stateMachine.AllTransitions
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
