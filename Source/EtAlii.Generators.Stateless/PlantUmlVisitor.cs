﻿namespace EtAlii.Generators.Stateless
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Antlr4.Runtime;

    /// <summary>
    /// An implementation of the visitor generated using the Antlr4 g4 parser and lexer.
    /// Antlr4 is very fascinating technology if you'd ask me...
    /// </summary>
    public class PlantUmlVisitor : PlantUmlParserBaseVisitor<object>
    {
        private readonly string _originalFileName;

        public PlantUmlVisitor(string originalFileName)
        {
            _originalFileName = originalFileName;
        }

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

            // If there is no classname defined in the diagram we'll need to come up with one ourselves.
            if (!settings.OfType<ClassNameSetting>().Any())
            {
                // Let's use a C# safe subset of the characters in the filename.
                var classNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(_originalFileName), "[^a-zA-Z0-9_]", "");
                settings = settings
                    .Concat(new [] { new ClassNameSetting(classNameFromFileName) })
                    .ToArray();
            }

            return new StateMachine(realHeaders, settings, stateFragments);
        }

        public override object VisitId(PlantUmlParser.IdContext context) => context.GetText();

        public override object VisitSetting_class(PlantUmlParser.Setting_classContext context) => new ClassNameSetting((string)VisitId(context.name));
        public override object VisitSetting_generate_partial(PlantUmlParser.Setting_generate_partialContext context) => new GeneratePartialClassSetting(true);
        public override object VisitSetting_namespace(PlantUmlParser.Setting_namespaceContext context) => new NamespaceSetting(context.@namespace().GetText());
        public override object VisitSetting_using(PlantUmlParser.Setting_usingContext context) => new UsingSetting(context.@namespace().GetText());

        public override object VisitNote_line(PlantUmlParser.Note_lineContext context) => new Header(context.GetText());

        public override object VisitTransition_details(PlantUmlParser.Transition_detailsContext context)
        {
            var triggerNameContext = context.trigger_name();
            var name = triggerNameContext?.GetText().Replace(" ", "");

            return new TransitionDetails(name, name != null);
        }

        public override object VisitParameters_definition_named(PlantUmlParser.Parameters_definition_namedContext context)
        {
            var parameters = new List<Parameter>();

            var types = context.parameter_type();
            var names = context.id();
            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var typeName = (string)VisitParameter_type(type);
                var name = (string)VisitId(names[i]);

                var position = SourcePosition.FromContext(type);
                parameters.Add(new Parameter(typeName, name, position));
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
                    var position = SourcePosition.FromContext(type);
                    return new Parameter(typeName, string.Empty, position);
                })
                .ToArray();
        }

        public override object VisitParameter_type(PlantUmlParser.Parameter_typeContext context)
        {
            var isArray = context.LBRACK() != null;
            return $"{string.Join(".", context.id().Select(id => (string)VisitId(id)))}{(isArray ? "[]" : "")}";
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
            var fallbackTriggerName = $"{(string)VisitId(context.from)}To{(string)VisitId(context.to)}";
            var from = (string)VisitId(context.from);
            var to = (string)VisitId(context.to);
            return BuildTransition(context, from, to, context.trigger_details(), context.transition_details(), fallbackTriggerName);
        }

        public override object VisitStates_transition_to_from(PlantUmlParser.States_transition_to_fromContext context)
        {
            var fallbackTriggerName = $"{(string)VisitId(context.from)}To{(string)VisitId(context.to)}";
            var from = (string)VisitId(context.from);
            var to = (string)VisitId(context.to);
            return BuildTransition(context, from, to, context.trigger_details(), context.transition_details(), fallbackTriggerName);
        }

        public override object VisitStates_transition_start_to(PlantUmlParser.States_transition_start_toContext context)
        {
            var fallbackTriggerName = $"{StatelessWriter.BeginStateName}To{(string)VisitId(context.to)}";
            var from = StatelessWriter.BeginStateName;
            var to = (string)VisitId(context.to);
            return BuildTransition(context, from, to, context.trigger_details(), context.transition_details(), fallbackTriggerName);
        }

        public override object VisitStates_transition_to_start(PlantUmlParser.States_transition_to_startContext context)
        {
            var fallbackTriggerName = $"{StatelessWriter.BeginStateName}To{(string)VisitId(context.to)}";
            var from = StatelessWriter.BeginStateName;
            var to = (string)VisitId(context.to);
            return BuildTransition(context, from, to, context.trigger_details(), context.transition_details(), fallbackTriggerName);
        }

        public override object VisitStates_transition_from_end(PlantUmlParser.States_transition_from_endContext context)
        {
            var fallbackTriggerName = $"{(string)VisitId(context.from)}To{StatelessWriter.EndStateName}";
            var from = (string)VisitId(context.from);
            var to = StatelessWriter.EndStateName;
            return BuildTransition(context, from, to, context.trigger_details(), context.transition_details(), fallbackTriggerName);
        }

        public override object VisitStates_transition_end_from(PlantUmlParser.States_transition_end_fromContext context)
        {
            var fallbackTriggerName = $"{(string)VisitId(context.from)}To{StatelessWriter.EndStateName}";
            var from = (string)VisitId(context.from);
            var to = StatelessWriter.EndStateName;
            return BuildTransition(context, from, to, context.trigger_details(), context.transition_details(), fallbackTriggerName);
        }

        private Transition BuildTransition(
            ParserRuleContext context,
            string from,
            string to,
            PlantUmlParser.Trigger_detailsContext triggerDetailsContext,
            PlantUmlParser.Transition_detailsContext transitionDetailsContext,
            string fallbackTriggerName)
        {
            var triggerDetails = triggerDetailsContext != null
                ? (TriggerDetails)VisitTrigger_details(triggerDetailsContext)
                : new TriggerDetails(false, Array.Empty<Parameter>());

            var transitionDetails = transitionDetailsContext != null
                ? (TransitionDetails)VisitTransition_details(transitionDetailsContext)
                : new TransitionDetails(fallbackTriggerName, false);

            if (!transitionDetails.HasConcreteName)
            {
                transitionDetails.Name = fallbackTriggerName;
            }

            var position = SourcePosition.FromContext(context);
            return new Transition(from, to, transitionDetails, triggerDetails, position);
        }

        public override object VisitStates_description(PlantUmlParser.States_descriptionContext context) => new StateDescription((string)VisitId(context.id()), context.text?.Text ?? string.Empty);

        public override object VisitStereotype(PlantUmlParser.StereotypeContext context)
        {
            return (StereoType)Enum.Parse(typeof(StereoType), context.GetText(), true);
        }

        public override object VisitState_definition_no_substates(PlantUmlParser.State_definition_no_substatesContext context)
        {
            var name = (string)VisitId(context.name);

            var stereoTypeContext = context.stereotype();
            var stereoType = stereoTypeContext != null
                ? (StereoType)VisitStereotype(stereoTypeContext)
                : StereoType.None;

            var position = SourcePosition.FromContext(context);
            return new SuperState(name, Array.Empty<StateFragment>(), position, stereoType);
        }

        public override object VisitState_definition_with_substates(PlantUmlParser.State_definition_with_substatesContext context)
        {
            var name = (string)VisitId(context.name);
            var stateFragments = context
                .states()?
                .Select(Visit)
                .OfType<StateFragment>()
                .ToArray() ?? Array.Empty<StateFragment>();

            var stereoTypeContext = context.stereotype();
            var stereoType = stereoTypeContext != null
                ? (StereoType)VisitStereotype(stereoTypeContext)
                : StereoType.None;

            var position = SourcePosition.FromContext(context.name);
            return new SuperState(name, stateFragments, position, stereoType);
        }
    }
}
