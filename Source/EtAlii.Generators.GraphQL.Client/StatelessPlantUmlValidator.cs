// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.GraphQL.Client
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// The central class responsible of validating both the PlantUML and Stateless specific requirements
    /// and express them using Roslyn Diagnostic instances.
    /// </summary>
    public class StatelessPlantUmlValidator
    {
        public void Validate(WriteContext context, List<Diagnostic> diagnostics)
        {
            CheckForStartStates(context, diagnostics);

            CheckForUnnamedParameters(context, diagnostics);

            CheckForUnnamedTriggers(context, diagnostics);

            CheckSubstatesEntryTransition(context, diagnostics);
        }

        private void CheckSubstatesEntryTransition(WriteContext context, List<Diagnostic> diagnostics)
        {
            var superStates = StateFragment.GetAllSuperStates(context.StateMachine.StateFragments);
            foreach (var superState in superStates)
            {
                var allSubstates = StateFragment.GetAllStates(superState.StateFragments);
                var allSubTransitions = StateFragment.GetAllTransitions(superState.StateFragments);
                var allTransitions = StateFragment.GetAllTransitions(context.StateMachine.StateFragments);

                var directTransitionsToSubState = allTransitions
                    .Where(t => !allSubTransitions.Contains(t))
                    .Where(t => allSubstates.Contains(t.To))
                    .ToArray();

                var transitionsToSuperState = allTransitions
                    .Where(t => t.To == superState.Name)
                    .ToArray();

                var superStateStartTransitions = superState.StateFragments
                    .OfType<Transition>()
                    .Where(t => t.From != t.To && t.From == SourceGenerator.BeginStateName)
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
                    var location = superState.Source.ToLocation(context.OriginalFileName);
                    var diagnostic = Diagnostic.Create(DiagnosticRule.SuperstateHasBothUnnamedAndDirectTransitionsDefined, location, superState.Source.Text);
                    diagnostics.Add(diagnostic);
                }

                // ~NAMED UNNAMED+ ~DIRECT
                if (!namedSuperStateStartTransitions.Any() && unnamedSuperStateStartTransitions.Any() && !directTransitionsToSubState.Any() && unnamedSuperStateStartTransitions.Length > 1)
                {
                    var location = superState.Source.ToLocation(context.OriginalFileName);
                    var diagnostic = Diagnostic.Create(DiagnosticRule.SuperstateHasMultipleUnnamedStartTransitionsDefined, location, superState.Source.Text);
                    diagnostics.Add(diagnostic);
                }
            }
        }

        private static void CheckForUnnamedTriggers(WriteContext context, List<Diagnostic> diagnostics)
        {
            var allTransitions = StateFragment.GetAllTransitions(context.StateMachine.StateFragments);
            var transitionsWithUnnamedTrigger = allTransitions
                .Where(t => !t.HasConcreteTriggerName)
                .ToArray();

            foreach (var transitionWithUnnamedTrigger in transitionsWithUnnamedTrigger)
            {
                var location = transitionWithUnnamedTrigger.Source.ToLocation(context.OriginalFileName);
                var diagnostic = Diagnostic.Create(DiagnosticRule.UnnamedTrigger, location, transitionWithUnnamedTrigger.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        private static void CheckForUnnamedParameters(WriteContext context, List<Diagnostic> diagnostics)
        {
            var allTransitions = StateFragment.GetAllTransitions(context.StateMachine.StateFragments);
            var unnamedParameters = allTransitions
                .Where(t => t.Parameters.Any(p => !p.HasName))
                .Select(t => t.Parameters.First(p => !p.HasName))
                .ToArray();

            foreach (var unnamedParameter in unnamedParameters)
            {
                var location = unnamedParameter.Source.ToLocation(context.OriginalFileName);
                var diagnostic = Diagnostic.Create(DiagnosticRule.UnnamedParameter, location, unnamedParameter.Source.Text);

                diagnostics.Add(diagnostic);
            }
        }

        private static void CheckForStartStates(WriteContext context, List<Diagnostic> diagnostics)
        {
            var allTransitions = StateFragment.GetAllTransitions(context.StateMachine.StateFragments);
            var startStates = allTransitions
                .Where(t => t.From == SourceGenerator.BeginStateName)
                .ToArray();
            if (startStates.Length == 0)
            {
                var location = Location.Create(context.OriginalFileName, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(DiagnosticRule.NoStartStatesDefined, location);
                diagnostics.Add(diagnostic);
            }
        }
    }
}
