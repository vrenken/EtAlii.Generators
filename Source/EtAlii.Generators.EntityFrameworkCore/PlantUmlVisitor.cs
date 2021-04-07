namespace EtAlii.Generators.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr4.Runtime;

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

            return new EntityModel(realHeaders, settings, stateFragments);
        }

        public override object VisitId(PlantUmlParser.IdContext context) => context.GetText();

        public override object VisitStateless_setting_class(PlantUmlParser.Stateless_setting_classContext context) => new ClassNameSetting((string)VisitId(context.name));
        public override object VisitStateless_setting_generate_partial(PlantUmlParser.Stateless_setting_generate_partialContext context) => new GeneratePartialClassSetting(true);
        public override object VisitStateless_setting_namespace(PlantUmlParser.Stateless_setting_namespaceContext context) => new NamespaceSetting(context.@namespace().GetText());
        public override object VisitStateless_setting_using(PlantUmlParser.Stateless_setting_usingContext context) => new UsingSetting(context.@namespace().GetText());

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
    }
}
