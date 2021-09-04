namespace EtAlii.Generators.PlantUml
{
    using System;
    using Antlr4.Runtime;

    public abstract class StateMachineLifetimeBase
    {

        public TriggerDetails BuildTriggerDetails(PlantUmlParser.Trigger_detailsContext context, PlantUmlVisitor visitor)
        {
            var parameters = Array.Empty<Parameter>();
            var parameterContext = context.parameters_definition();
            if (parameterContext != null)
            {
                parameters = (Parameter[])visitor.VisitParameters_definition(context.parameters_definition());
            }

            var isAsync = context.ASYNC() != null;

            return new TriggerDetails(isAsync, parameters);
        }

        public TransitionDetails BuildTransitionDetails(PlantUmlParser.Transition_detailsContext context)
        {
            var triggerNameContext = context.trigger_name();
            var name = triggerNameContext?.GetText().Replace(" ", "");

            return new TransitionDetails(name, name != null);
        }

        public Transition BuildTransition(
            ParserRuleContext context,
            string from,
            string to,
            PlantUmlParser.Trigger_detailsContext triggerDetailsContext,
            PlantUmlParser.Transition_detailsContext transitionDetailsContext,
            string fallbackTriggerName,
            PlantUmlVisitor visitor)
        {
            var triggerDetails = triggerDetailsContext != null
                ? BuildTriggerDetails(triggerDetailsContext, visitor)
                : new TriggerDetails(false, Array.Empty<Parameter>());

            var transitionDetails = transitionDetailsContext != null
                ? BuildTransitionDetails(transitionDetailsContext)
                : new TransitionDetails(fallbackTriggerName, false);

            if (!transitionDetails.HasConcreteName)
            {
                transitionDetails.Name = fallbackTriggerName;
            }

            var position = SourcePosition.FromContext(context);
            return new Transition(from, to, transitionDetails, triggerDetails, position);
        }
    }
}
