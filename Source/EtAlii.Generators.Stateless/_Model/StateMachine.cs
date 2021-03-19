namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Linq;

    public class StateMachine
    {
        public string Namespace => Settings.OfType<NamespaceSetting>().Single().Value;
        public string Class => Settings.OfType<ClassStatelessSetting>().Single().Value;
        public bool GeneratePartialClass => Settings.OfType<GeneratePartialClassSetting>().SingleOrDefault()?.Value ?? false;
        public string[] Usings => Settings.OfType<UsingSetting>().Select(s => s.Value).ToArray();

        public List<Header> Headers { get; } = new();
        public List<Setting> Settings { get; } = new();
        public List<StateFragment> StateFragments { get; } = new();
    }
}
