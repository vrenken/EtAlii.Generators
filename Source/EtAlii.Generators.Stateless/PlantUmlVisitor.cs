namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An implementation of the visitor generated using the Antlr4 g4 parser and lexer.
    /// Antlr4 is very fascinating technology if you'd ask me...
    /// </summary>
    public class PlantUmlVisitor : PlantUmlParserBaseVisitor<object>
    {
        public override object VisitState_machine(PlantUmlParser.State_machineContext context)
        {
            var stateMachine = new StateMachine();

            var headers = context
                .header_lines()
                .Select(Visit)
                .ToArray();

            foreach (var header in headers)
            {
                switch (header)
                {
                    case Setting roslynSetting: stateMachine.Settings.Add(roslynSetting); break;
                    case Header realHeader: stateMachine.Headers.Add(realHeader); break;
                }
            }
            var stateFragments = context
                .states()
                .Select(Visit)
                .OfType<StateFragment>();
            stateMachine.StateFragments.AddRange(stateFragments);

            return stateMachine;
        }

        public override object VisitStateless_setting_class(PlantUmlParser.Stateless_setting_classContext context) => new ClassStatelessSetting(context.name.Text);
        public override object VisitStateless_setting_generate_partial(PlantUmlParser.Stateless_setting_generate_partialContext context) => new GeneratePartialClassSetting(true);
        public override object VisitStateless_setting_namespace(PlantUmlParser.Stateless_setting_namespaceContext context) => new NamespaceSetting(context.@namespace().GetText());
        public override object VisitStateless_setting_using(PlantUmlParser.Stateless_setting_usingContext context) => new UsingSetting(context.@namespace().GetText());

        public override object VisitNote_line(PlantUmlParser.Note_lineContext context) => new Header(context.GetText());

        public override object VisitTransition_details(PlantUmlParser.Transition_detailsContext context)
        {
            var isAsync = false;
            var parameters = Array.Empty<Parameter>();

            var triggerDefinitionContext = context.trigger_definition();
            if (triggerDefinitionContext != null)
            {
                var triggerDefinition = (TriggerDefinition)VisitTrigger_definition(triggerDefinitionContext);
                isAsync = triggerDefinition.IsAsync;
                parameters = triggerDefinition.Parameters;
            }
            var name = context.transition_details_id().GetText().Replace(" ", "");

            return new TransitionDetails(name, isAsync, parameters);
        }

        public override object VisitParameters_definition_named(PlantUmlParser.Parameters_definition_namedContext context)
        {
            var parameters = new List<Parameter>();
            var ids = context.ID();
            for (var i = 0; i < ids.Length; i += 2)
            {
                var type = ids[i];
                var name = ids[i + 1];

                parameters.Add(new Parameter(type.GetText(), name.GetText(), type.Symbol.Line, type.Symbol.Column));
            }

            return parameters.ToArray();
        }

        public override object VisitParameters_definition_unnamed(PlantUmlParser.Parameters_definition_unnamedContext context)
        {
            return context
                .ID()
                .Select(id => new Parameter(id.GetText(), null, id.Symbol.Line, id.Symbol.Column))
                .ToArray();
        }

        public override object VisitTrigger_definition(PlantUmlParser.Trigger_definitionContext context)
        {
            var parameters = Array.Empty<Parameter>();
            var parameterContext = context.parameters_definition();
            if (parameterContext != null)
            {
                parameters = (Parameter[])VisitParameters_definition(context.parameters_definition());
            }

            var isAsync = context.ASYNC() != null;

            return new TriggerDefinition(isAsync, parameters);
        }

        public override object VisitStates_transition_from_to(PlantUmlParser.States_transition_from_toContext context)
        {
            TransitionDetails transitionDetails;

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
            }
            else
            {
                var name = $"{context.from.Text}To{context.to.Text}";
                transitionDetails = new TransitionDetails(name, false, Array.Empty<Parameter>());
            }
            return new StateTransition(context.from.Text, context.to.Text, transitionDetails);
        }

        public override object VisitStates_transition_to_from(PlantUmlParser.States_transition_to_fromContext context)
        {
            TransitionDetails transitionDetails;

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
            }
            else
            {
                var name = $"{context.from.Text}To{context.to.Text}";
                transitionDetails = new TransitionDetails(name, false, Array.Empty<Parameter>());
            }
            return new StateTransition(context.from.Text, context.to.Text, transitionDetails);
        }

        public override object VisitStates_transition_start_to(PlantUmlParser.States_transition_start_toContext context)
        {
            TransitionDetails transitionDetails;

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
            }
            else
            {
                var name = $"StartTo{context.to.Text}";
                transitionDetails = new TransitionDetails(name, false, Array.Empty<Parameter>());
            }
            return new StateTransition("None", context.to.Text, transitionDetails);
        }

        public override object VisitStates_transition_to_start(PlantUmlParser.States_transition_to_startContext context)
        {
            TransitionDetails transitionDetails;

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
            }
            else
            {
                var name = $"StartTo{context.to.Text}";
                transitionDetails = new TransitionDetails(name, false, Array.Empty<Parameter>());
            }

            return new StateTransition("None", context.to.Text, transitionDetails);
        }

        public override object VisitStates_transition_from_end(PlantUmlParser.States_transition_from_endContext context)
        {
            TransitionDetails transitionDetails;

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
            }
            else
            {
                var name = $"{context.from.Text}ToEnd";
                transitionDetails = new TransitionDetails(name, false, Array.Empty<Parameter>());
            }

            return new EndTransition(context.from.Text, transitionDetails);
        }

        public override object VisitStates_transition_end_from(PlantUmlParser.States_transition_end_fromContext context)
        {
            TransitionDetails transitionDetails;

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
            }
            else
            {
                var name = $"{context.from.Text}ToEnd";
                transitionDetails = new TransitionDetails(name, false, Array.Empty<Parameter>());
            }

            return new EndTransition(context.from.Text, transitionDetails);
        }

        public override object VisitStates_description(PlantUmlParser.States_descriptionContext context) => new StateDescription(context.ID().GetText(), context.text?.Text ?? string.Empty);
    }
}
