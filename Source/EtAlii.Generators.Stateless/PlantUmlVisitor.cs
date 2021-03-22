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
            var headers = context
                .header_lines()
                .Select(Visit)
                .ToArray();

            var realHeaders = headers.OfType<Header>().ToArray();
            var settings = headers.OfType<Setting>().ToArray();

            var stateFragments = context
                .states()
                .Select(Visit)
                .OfType<StateFragment>()
                .ToArray();

            return new StateMachine(realHeaders, settings, stateFragments);
        }

        public override object VisitStateless_setting_class(PlantUmlParser.Stateless_setting_classContext context) => new ClassNameSetting(context.name.Text);
        public override object VisitStateless_setting_generate_partial(PlantUmlParser.Stateless_setting_generate_partialContext context) => new GeneratePartialClassSetting(true);
        public override object VisitStateless_setting_namespace(PlantUmlParser.Stateless_setting_namespaceContext context) => new NamespaceSetting(context.@namespace().GetText());
        public override object VisitStateless_setting_using(PlantUmlParser.Stateless_setting_usingContext context) => new UsingSetting(context.@namespace().GetText());

        public override object VisitNote_line(PlantUmlParser.Note_lineContext context) => new Header(context.GetText());

        public override object VisitTransition_details(PlantUmlParser.Transition_detailsContext context)
        {
            var isAsync = false;
            var parameters = Array.Empty<Parameter>();

            var triggerDetailsContext = context.trigger_details();
            if (triggerDetailsContext != null)
            {
                var triggerDetails = (TriggerDetails)VisitTrigger_details(triggerDetailsContext);
                isAsync = triggerDetails.IsAsync;
                parameters = triggerDetails.Parameters;
            }

            var triggerNameContext = context.trigger_name();
            var name = triggerNameContext?.GetText().Replace(" ", "");

            return new TransitionDetails(name, isAsync, parameters, name != null);
        }

        public override object VisitParameters_definition_named(PlantUmlParser.Parameters_definition_namedContext context)
        {
            var parameters = new List<Parameter>();

            var types = context.parameter_type();
            var names = context.ID();
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var typeName = (string)VisitParameter_type(type);
                var name = names[i];

                var position = new SourcePosition(type.Start.Line, type.Start.Column, type.GetText());
                parameters.Add(new Parameter(typeName, name.GetText(), position));
            }

            return parameters.ToArray();
        }

        public override object VisitParameters_definition_unnamed(PlantUmlParser.Parameters_definition_unnamedContext context)
        {
            return context
                .parameter_type()
                .Select(type =>
                {
                    var typeName = (string)VisitParameter_type(type);
                    return new Parameter(typeName, string.Empty, new SourcePosition(type.Start.Line, type.Start.Column, type.GetText()));
                })
                .ToArray();
        }

        public override object VisitParameter_type(PlantUmlParser.Parameter_typeContext context)
        {
            var isArray = context.LBRACK() != null;
            return $"{string.Join(".", context.ID().Select(id => id.GetText()))}{(isArray ? "[]" : "")}";
        }

        public override object VisitTrigger_details(PlantUmlParser.Trigger_detailsContext context)
        {
            var parameters = Array.Empty<Parameter>();
            var parameterContext = context.parameters_definition();
            if (parameterContext != null)
            {
                parameters = (Parameter[])VisitParameters_definition(context.parameters_definition());
            }

            var isAsync = context.ASYNC() != null;

            return new TriggerDetails(isAsync, parameters);
        }

        public override object VisitStates_transition_from_to(PlantUmlParser.States_transition_from_toContext context)
        {
            TransitionDetails transitionDetails;
            var fallbackTriggerName = $"{context.from.Text}To{context.to.Text}";

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
                if (!transitionDetails.HasConcreteName)
                {
                    transitionDetails.Name = fallbackTriggerName;
                }
            }
            else
            {
                transitionDetails = new TransitionDetails(fallbackTriggerName, false, Array.Empty<Parameter>(), false);
            }
            var position = new SourcePosition(context.Start.Line, context.Start.Column, context.GetText());
            return new StateTransition(context.from.Text, context.to.Text, transitionDetails, position);
        }

        public override object VisitStates_transition_to_from(PlantUmlParser.States_transition_to_fromContext context)
        {
            TransitionDetails transitionDetails;
            var fallbackTriggerName = $"{context.from.Text}To{context.to.Text}";

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
                if (!transitionDetails.HasConcreteName)
                {
                    transitionDetails.Name = fallbackTriggerName;
                }
            }
            else
            {
                transitionDetails = new TransitionDetails(fallbackTriggerName, false, Array.Empty<Parameter>(), false);
            }
            var position = new SourcePosition(context.Start.Line, context.Start.Column, context.GetText());
            return new StateTransition(context.from.Text, context.to.Text, transitionDetails, position);
        }

        public override object VisitStates_transition_start_to(PlantUmlParser.States_transition_start_toContext context)
        {
            TransitionDetails transitionDetails;
            var fallbackTriggerName = $"StartTo{context.to.Text}";

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
                if (!transitionDetails.HasConcreteName)
                {
                    transitionDetails.Name = fallbackTriggerName;
                }
            }
            else
            {
                transitionDetails = new TransitionDetails(fallbackTriggerName, false, Array.Empty<Parameter>(), false);
            }
            var position = new SourcePosition(context.Start.Line, context.Start.Column, context.GetText());
            return new StateTransition(SourceGenerator.BeginStateName, context.to.Text, transitionDetails, position);
        }

        public override object VisitStates_transition_to_start(PlantUmlParser.States_transition_to_startContext context)
        {
            TransitionDetails transitionDetails;
            var fallbackTriggerName = $"StartTo{context.to.Text}";

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
                if (!transitionDetails.HasConcreteName)
                {
                    transitionDetails.Name = fallbackTriggerName;
                }
            }
            else
            {
                transitionDetails = new TransitionDetails(fallbackTriggerName, false, Array.Empty<Parameter>(), false);
            }

            var position = new SourcePosition(context.Start.Line, context.Start.Column, context.GetText());
            return new StateTransition(SourceGenerator.BeginStateName, context.to.Text, transitionDetails, position);
        }

        public override object VisitStates_transition_from_end(PlantUmlParser.States_transition_from_endContext context)
        {
            TransitionDetails transitionDetails;
            var fallbackTriggerName = $"{context.from.Text}ToEnd";

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
                if (!transitionDetails.HasConcreteName)
                {
                    transitionDetails.Name = fallbackTriggerName;
                }
            }
            else
            {
                transitionDetails = new TransitionDetails(fallbackTriggerName, false, Array.Empty<Parameter>(), false);
            }

            var position = new SourcePosition(context.Start.Line, context.Start.Column, context.GetText());
            return new StateTransition(context.from.Text, SourceGenerator.EndStateName, transitionDetails, position);
        }

        public override object VisitStates_transition_end_from(PlantUmlParser.States_transition_end_fromContext context)
        {
            TransitionDetails transitionDetails;
            var fallbackTriggerName = $"{context.from.Text}ToEnd";

            var transitionDetailsContext = context.transition_details();
            if (transitionDetailsContext != null)
            {
                transitionDetails = (TransitionDetails)VisitTransition_details(transitionDetailsContext);
                if (!transitionDetails.HasConcreteName)
                {
                    transitionDetails.Name = fallbackTriggerName;
                }
            }
            else
            {
                transitionDetails = new TransitionDetails(fallbackTriggerName, false, Array.Empty<Parameter>(), false);
            }

            var position = new SourcePosition(context.Start.Line, context.Start.Column, context.GetText());
            return new StateTransition(context.from.Text, SourceGenerator.EndStateName, transitionDetails, position);
        }

        public override object VisitStates_description(PlantUmlParser.States_descriptionContext context) => new StateDescription(context.ID().GetText(), context.text?.Text ?? string.Empty);

        public override object VisitState_definition(PlantUmlParser.State_definitionContext context)
        {
            var name = context.name.Text;
            var stateFragments = context
                .states()
                .Select(Visit)
                .OfType<StateFragment>()
                .ToArray();

            return new SuperState(name, stateFragments);
        }
    }
}
