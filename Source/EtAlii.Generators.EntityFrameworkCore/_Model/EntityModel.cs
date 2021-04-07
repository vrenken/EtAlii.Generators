namespace EtAlii.Generators.EntityFrameworkCore
{
    using System.Linq;

    public class EntityModel
    {
        /// <summary>
        /// The namespace that should be used when creating source code files.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// The root class name.
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// When set to true a partial class will be created.
        /// </summary>
        public bool GeneratePartialClass { get; private set; }

        /// <summary>
        /// The namespaces that should be added as usings at the root of the file.
        /// </summary>
        public string[] Usings { get; private set; }

        /// <summary>
        /// Additional headers from the Plant UML file.
        /// </summary>
        public Header[] Headers { get; }

        /// <summary>
        /// All settings acquired from the Plant UML file.
        /// </summary>
        public Setting[] Settings { get; private set; }

        /// <summary>
        /// The root state fragments (i.e. the ones that don't have a super state).
        /// </summary>
        public StateFragment[] StateFragments { get; }

        public EntityModel(Header[] headers, Setting[] settings, StateFragment[] stateFragments)
        {
            Headers = headers;
            Settings = settings;
            StateFragments = stateFragments;
            Update();
        }

        public void AddSettings(Setting setting)
        {
            Settings = Settings
                .Concat(new[] {setting})
                .ToArray();
            Update();
        }

        private void Update()
        {
            ClassName = Settings
                .OfType<ClassNameSetting>()
                .Single().Value;
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
