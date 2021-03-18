namespace EtAlii.Generators.Stateless
{
    public class NamespaceRoslynSetting : RoslynSetting
    {
        public string Namespace { get; }

        public NamespaceRoslynSetting(string @namespace)
        {
            Namespace = @namespace;
        }
    }
}
