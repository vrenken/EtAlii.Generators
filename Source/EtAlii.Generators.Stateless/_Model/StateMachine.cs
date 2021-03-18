namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Linq;

    public class StateMachine
    {
        public string Namespace => Settings.OfType<NamespaceRoslynSetting>().Single().Namespace;
        public string Class => Settings.OfType<ClassRoslynSetting>().Single().Class;
        public bool GeneratePartialClass => Settings.OfType<GeneratePartialNamespaceRoslynSetting>().SingleOrDefault()?.GeneratePartial ?? false;

        public List<Header> Headers { get; } = new();
        public List<RoslynSetting> Settings { get; } = new();
        public List<StateFragment> StateFragments { get; } = new();
    }
}
