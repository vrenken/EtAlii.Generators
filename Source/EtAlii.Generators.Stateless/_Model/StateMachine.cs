namespace EtAlii.Generators.Stateless
{
    using System.Linq;

    public class StateMachine
    {
        public string Namespace => Settings.OfType<NamespaceSetting>().Single().Value;
        public string ClassName => Settings.OfType<ClassNameSetting>().Single().Value;
        public bool GeneratePartialClass => Settings.OfType<GeneratePartialClassSetting>().SingleOrDefault()?.Value ?? false;
        public string[] Usings => Settings.OfType<UsingSetting>().Select(s => s.Value).ToArray();

        public Header[] Headers { get; }
        public Setting[] Settings { get; private set; }
        public StateFragment[] StateFragments { get; }

        public StateMachine(Header[] headers, Setting[] settings, StateFragment[] stateFragments)
        {
            Headers = headers;
            Settings = settings;
            StateFragments = stateFragments;
        }

        public void AddSettings(Setting setting)
        {
            Settings = Settings
                .Concat(new[] {setting})
                .ToArray();
        }
    }
}
