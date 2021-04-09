namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.Linq;

    public class EntityModel
    {
        /// <summary>
        /// The namespace that should be used when creating source code files.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// The name of the base entity to use.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// The name of the DbContext instance to generate.
        /// </summary>
        public string DbContextName { get; }

        /// <summary>
        /// When set to true a partial class will be created.
        /// </summary>
        public bool GeneratePartialClass { get; }

        /// <summary>
        /// The namespaces that should be added as usings at the root of the file.
        /// </summary>
        public string[] Usings { get; }

        /// <summary>
        /// Additional headers from the Plant UML file.
        /// </summary>
        public Header[] Headers { get; }

        /// <summary>
        /// All settings acquired from the Plant UML file.
        /// </summary>
        public Setting[] Settings { get; }

        /// <summary>
        /// The classes as defined in the PlantUML diagram.
        /// </summary>
        public Class[] Classes { get; }

        public Relation[] Relations { get; }

        public EntityModel(Header[] headers, Setting[] settings, Class[] classes, Relation[] relations)
        {
            Headers = headers;
            Settings = settings;
            Classes = classes;
            Relations = relations;

            DbContextName = Settings
                .OfType<DbContextNameSetting>()
                .SingleOrDefault()?.Value;
            EntityName = Settings
                .OfType<EntityNameSetting>()
                .SingleOrDefault()?.Value;
            Namespace = Settings
                .OfType<NamespaceSetting>()
                .Single().Value;
            Usings = Settings
                .OfType<UsingSetting>()
                .Select(s => s.Value)
                .ToArray();
            GeneratePartialClass = Settings
                .OfType<GeneratePartialClassSetting>()
                .SingleOrDefault()?.Value ?? false;
        }
    }
}
