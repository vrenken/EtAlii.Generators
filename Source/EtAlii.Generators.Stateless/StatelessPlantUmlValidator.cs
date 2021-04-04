// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// The central class responsible of validating both the PlantUML and Stateless specific requirements
    /// and express them using Roslyn Diagnostic instances.
    /// </summary>
    public class StatelessPlantUmlValidator : IValidator<StateMachine>
    {
        public void Validate(StateMachine instance, string originalFileName, List<Diagnostic> diagnostics)
        {
            CheckForStartStates(instance, originalFileName, diagnostics);

            CheckForDuplicateTriggers(instance, originalFileName, diagnostics);

            CheckForUnnamedParameters(instance, originalFileName, diagnostics);

            CheckForUnnamedTriggers(instance, originalFileName, diagnostics);

            CheckSubstatesEntryTransition(instance, originalFileName, diagnostics);
        }

        private void CheckForDuplicateTriggers(StateMachine stateMachine, string originalFileName, List<Diagnostic> diagnostics)
        {
            var transitionsWithDuplicateTriggers = StateFragment
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
                var location = source.ToLocation(originalFileName);
                var diagnostic = Diagnostic.Create(GeneratorRule.DuplicateTriggers, location, transitionWithDuplicateTriggers.Trigger, transitionWithDuplicateTriggers.Count, transitionWithDuplicateTriggers.State);

                diagnostics.Add(diagnostic);
            }
        }

        private void CheckSubstatesEntryTransition(StateMachine stateMachine, string originalFileName, List<Diagnostic> diagnostics)
        {
            var superStates = StateFragment.GetAllSuperStates(stateMachine.StateFragments);
            foreach (var superState in superStates)
            {
                var allSubstates = StateFragment.GetAllStates(superState.StateFragments);
                var allSubTransitions = StateFragment.GetAllTransitions(superState.StateFragments);
                var allTransitions = StateFragment.GetAllTransitions(stateMachine.StateFragments);

                var directTransitionsToSubState = allTransitions
                    .Where(t => !allSubTransitions.Contains(t))
                    .Where(t => allSubstates.Contains(t.To))
                    .ToArray();

                var transitionsToSuperState = allTransitions
                    .Where(t => t.To == superState.Name)
                    .ToArray();

                var superStateStartTransitions = superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From != t.To && t.From == StatelessWriter.BeginStateName)
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
                    var location = superState.Source.ToLocation(originalFileName);
                    var diagnostic = Diagnostic.Create(GeneratorRule.SuperstateHasBothUnnamedAndDirectTransitionsDefined, location, superState.Source.Text);
                    diagnostics.Add(diagnostic);
                }

                // ~NAMED UNNAMED+ ~DIRECT
                if (!namedSuperStateStartTransitions.Any() && unnamedSuperStateStartTransitions.Any() && !directTransitionsToSubState.Any() && unnamedSuperStateStartTransitions.Length > 1)
                {
                    var location = superState.Source.ToLocation(originalFileName);
                    var diagnostic = Diagnostic.Create(GeneratorRule.SuperstateHasMultipleUnnamedStartTransitionsDefined, location, superState.Source.Text);
                    diagnostics.Add(diagnostic);
                }
            }
        }

        private static void CheckForUnnamedTriggers(StateMachine stateMachine, string originalFileName, List<Diagnostic> diagnostics)
        {
            var allTransitions = StateFragment.GetAllTransitions(stateMachine.StateFragments);
            var transitionsWithUnnamedTrigger = allTransitions
                .Where(t => !t.HasConcreteTriggerName)
                .ToArray();

            foreach (var transitionWithUnnamedTrigger in transitionsWithUnnamedTrigger)
            {
                var location = transitionWithUnnamedTrigger.Source.ToLocation(originalFileName);
                var diagnostic = Diagnostic.Create(GeneratorRule.UnnamedTrigger, location, transitionWithUnnamedTrigger.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        private static void CheckForUnnamedParameters(StateMachine stateMachine, string originalFileName, List<Diagnostic> diagnostics)
        {
            var allTransitions = StateFragment.GetAllTransitions(stateMachine.StateFragments);
            var unnamedParameters = allTransitions
                .Where(t => t.Parameters.Any(p => !p.HasName))
                .Select(t => t.Parameters.First(p => !p.HasName))
                .ToArray();

            foreach (var unnamedParameter in unnamedParameters)
            {
                var location = unnamedParameter.Source.ToLocation(originalFileName);
                var diagnostic = Diagnostic.Create(GeneratorRule.UnnamedParameter, location, unnamedParameter.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        private static void CheckForStartStates(StateMachine stateMachine, string originalFileName, List<Diagnostic> diagnostics)
        {
            var allTransitions = StateFragment.GetAllTransitions(stateMachine.StateFragments);
            var startStates = allTransitions
                .Where(t => t.From == StatelessWriter.BeginStateName)
                .ToArray();
            if (startStates.Length == 0)
            {
                var location = Location.Create(originalFileName, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(GeneratorRule.NoStartStatesDefined, location);
                diagnostics.Add(diagnostic);
            }
        }
    }
}
