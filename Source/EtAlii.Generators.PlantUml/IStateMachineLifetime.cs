namespace EtAlii.Generators.PlantUml
{
    using Antlr4.Runtime;

    public interface IStateMachineLifetime
    {
        string BeginStateName { get; }
        string EndStateName { get; }

        TriggerDetails BuildTriggerDetails(PlantUmlParser.Trigger_detailsContext context, PlantUmlVisitor visitor);
        TransitionDetails BuildTransitionDetails(PlantUmlParser.Transition_detailsContext context);

        Transition BuildTransition(
            ParserRuleContext context,
            string from,
            string to,
            PlantUmlParser.Trigger_detailsContext triggerDetailsContext,
            PlantUmlParser.Transition_detailsContext transitionDetailsContext,
            string fallbackTriggerName,
            PlantUmlVisitor visitor);
    }
}
