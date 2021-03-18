namespace EtAlii.Generators.Stateless
{
    public class ClassRoslynSetting : RoslynSetting
    {
        public string Class { get; }

        public ClassRoslynSetting(string @class)
        {
            Class = @class;
        }
    }
}
