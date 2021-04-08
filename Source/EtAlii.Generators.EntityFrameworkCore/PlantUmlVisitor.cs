namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.Linq;

    /// <summary>
    /// An implementation of the visitor generated using the Antlr4 g4 parser and lexer.
    /// Antlr4 is very fascinating technology if you'd ask me...
    /// </summary>
    public class PlantUmlVisitor : PlantUmlParserBaseVisitor<object>
    {
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
                .OfType<StateFragment>()
                .ToArray();

            return new EntityModel(realHeaders, settings, items);
        }

        public override object VisitId(PlantUmlParser.IdContext context) => context.GetText();

        public override object VisitSetting_entity(PlantUmlParser.Setting_entityContext context) => new EntityNameSetting((string)VisitId(context.name));
        public override object VisitSetting_dbcontext(PlantUmlParser.Setting_dbcontextContext context) => new DbContextNameSetting((string)VisitId(context.name));
        public override object VisitSetting_generate_partial(PlantUmlParser.Setting_generate_partialContext context) => new GeneratePartialClassSetting(true);
        public override object VisitSetting_namespace(PlantUmlParser.Setting_namespaceContext context) => new NamespaceSetting(context.@namespace().GetText());
        public override object VisitSetting_using(PlantUmlParser.Setting_usingContext context) => new UsingSetting(context.@namespace().GetText());

    }
}
