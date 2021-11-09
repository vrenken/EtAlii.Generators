namespace EtAlii.Generators.PlantUml
{
    using System.Linq;

    public class StateMachine
    {
        /// <summary>
        /// The namespace that should be used when creating source code files.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// The root class name.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// When set to true a partial class will be created.
        /// </summary>
        public bool GeneratePartialClass { get; }

        /// <summary>
        /// When set to true the choices per state will be aggregated and made available.
        /// </summary>
        public bool GenerateTriggerChoices { get; }

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
        /// The root state fragments (i.e. the ones that don't have a super state).
        /// </summary>
        public StateFragment[] StateFragments { get; }

        /// <summary>
        /// The states that make up the state machine. In hierarchical form.
        /// </summary>
        public State[] HierarchicalStates { get; }

        /// <summary>
        /// The states that make up the state machine. In sequential form.
        /// </summary>
        public State[] SequentialStates { get; }

        public StateMachine(Header[] headers, Setting[] settings, StateFragment[] stateFragments, State[] hierarchicalStates, State[] sequentialStates)
        {
            Headers = headers;
            Settings = settings;
            StateFragments = stateFragments;
            HierarchicalStates = hierarchicalStates;
            SequentialStates = sequentialStates;

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
            GenerateTriggerChoices = Settings
                .OfType<GenerateTriggerChoices>()
                .SingleOrDefault()?.Value ?? true;
        }
    }
}
