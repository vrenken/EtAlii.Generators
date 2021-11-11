namespace EtAlii.Generators.PlantUml
{
    using System.Linq;

    public partial class StateHierarchyBuilder
    {
        private bool HasOnlyAsyncOutboundTransitions(Transition[] allTransitions, SuperState[] allSuperStates, Transition[] outboundTransitions, string state)
        {
            var superState = allSuperStates.SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = GetAllSubStates(superState);
                var directTransitionsFromSubState = allTransitions
                    .Where(t => t.To != state && !allSubstates.Contains(t.To) && allSubstates.Contains(t.From))
                    .ToArray();

                outboundTransitions = outboundTransitions
                    .Concat(directTransitionsFromSubState)
                    .ToArray();
            }

            return
                outboundTransitions.Any() &&
                outboundTransitions.All(t => t.IsAsync) &&
                state != _lifetime.BeginStateName &&
                state != _lifetime.EndStateName;
        }

        private bool HasOnlyAsyncInboundTransitions(Transition[] allTransitions, SuperState[] allSuperStates, Transition[] inboundTransitions, string state)
        {
            var superState = allSuperStates
                .SingleOrDefault(ss => ss.Name == state);
            if (superState != null)
            {
                var allSubstates = GetAllSubStates(superState);
                var directTransitionsToSubState = allTransitions
                    .Where(t => t.From != state && allSubstates.Contains(t.To) && !allSubstates.Contains(t.From))
                    .ToArray();

                inboundTransitions = inboundTransitions
                    .Concat(directTransitionsToSubState)
                    .ToArray();
            }

            return
                inboundTransitions.Any() &&
                inboundTransitions.All(t => t.IsAsync) &&
                state != _lifetime.BeginStateName &&
                state != _lifetime.EndStateName;
        }
    }
}
