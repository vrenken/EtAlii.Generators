namespace EtAlii.Generators.Stateless
{
    public class GeneratePartialNamespaceRoslynSetting : RoslynSetting
    {
        public bool GeneratePartial { get; }

        public GeneratePartialNamespaceRoslynSetting(bool generatePartial)
        {
            GeneratePartial = generatePartial;
        }
    }
}
