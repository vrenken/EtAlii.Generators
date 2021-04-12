namespace EtAlii.Generators.EntityFrameworkCore
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

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

        public override object VisitModel(PlantUmlParser.ModelContext context)
        {
            var headers = context
                .header_items()
                .Select(Visit)
                .ToArray();

            var realHeaders = headers.OfType<Header>().ToArray();
            var settings = headers.OfType<Setting>().ToArray();

            var items = context
                .model_items()
                .Select(Visit)
                .ToArray();

            var classes = items
                .OfType<Class>()
                .ToArray();

            var relations = items
                .OfType<Relation>()
                .ToArray();

            // If there is no DbContext name defined in the diagram we'll need to come up with one ourselves.
            if (!settings.OfType<DbContextNameSetting>().Any())
            {
                // Let's use a C# safe subset of the characters in the filename.
                var dbContextNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(_originalFileName), "[^a-zA-Z0-9_]", "");
                settings = settings
                    .Concat(new []{ new DbContextNameSetting(dbContextNameFromFileName) })
                    .ToArray();
            }

            return new EntityModel(realHeaders, settings, classes, relations);
        }

        public override object VisitRelation_mapping(PlantUmlParser.Relation_mappingContext context)
        {
            var fromProperty = (string)VisitId(context.from);
            var toProperty = (string)VisitId(context.to);
            var position = SourcePosition.FromContext(context);
            return new RelationMapping(fromProperty, toProperty, position);
        }

        public override object VisitRelation_cardinality(PlantUmlParser.Relation_cardinalityContext context)
        {
            return context.GetText() switch
            {
                "\"0..1\"" or "'0..1'" => Cardinality.NoneOrOne,
                "\"0..n\"" or "'0..n'" => Cardinality.NoneOrMore,
                "\"1\"" or "'1'" => Cardinality.One,
                "\"1..n\"" or "'1..n'" => Cardinality.OneOrMore,
                var text => throw new InvalidOperationException($"Unable to visit cardinality: {text}")
            };
        }

        public override object VisitRelation(PlantUmlParser.RelationContext context)
        {
            var from = (string)VisitId(context.from);
            var fromCardinality = (Cardinality)VisitRelation_cardinality(context.from_cardinality);
            var to = (string)VisitId(context.to);
            var toCardinality = (Cardinality)VisitRelation_cardinality(context.to_cardinality);
            var relationMapping = (RelationMapping)VisitRelation_mapping(context.relation_mapping());
            var position = SourcePosition.FromContext(context);
            return new Relation(from, fromCardinality, to, toCardinality, relationMapping, position);
        }

        public override object VisitClass(PlantUmlParser.ClassContext context)
        {
            var name = (string)VisitId(context.name);
            var position = SourcePosition.FromContext(context);

            var classMapping = context.class_mapping() is var classMappingContext
                ? (ClassMapping)VisitClass_mapping(classMappingContext)
                : null;

            var properties = context
                .class_property()
                .Select(VisitClass_property)
                .Cast<Property>()
                .ToArray();
            return new Class(name, properties, classMapping, position);
        }

        public override object VisitClass_mapping(PlantUmlParser.Class_mappingContext context)
        {
            var name = (string)VisitId(context.name);
            var position = SourcePosition.FromContext(context);
            return new ClassMapping(name, position);
        }

        public override object VisitClass_property(PlantUmlParser.Class_propertyContext context)
        {
            var name = (string)VisitId(context.name);
            var type = (string)VisitId(context.type);
            if (context.is_array != null)
            {
                type = $"{type}[]";
            }

            var position = SourcePosition.FromContext(context);
            return new Property(name, type, position);
        }

        public override object VisitId(PlantUmlParser.IdContext context) => context.GetText();

        public override object VisitSetting_entity(PlantUmlParser.Setting_entityContext context) => new EntityNameSetting((string)VisitId(context.name));
        public override object VisitSetting_dbcontext(PlantUmlParser.Setting_dbcontextContext context) => new DbContextNameSetting((string)VisitId(context.name));
        public override object VisitSetting_interface(PlantUmlParser.Setting_interfaceContext context) => new InterfaceNameSetting((string)VisitId(context.name));
        public override object VisitSetting_generate_partial(PlantUmlParser.Setting_generate_partialContext context) => new GeneratePartialClassSetting(true);
        public override object VisitSetting_namespace(PlantUmlParser.Setting_namespaceContext context) => new NamespaceSetting(context.@namespace().GetText());
        public override object VisitSetting_using(PlantUmlParser.Setting_usingContext context) => new UsingSetting(context.@namespace().GetText());

    }
}
