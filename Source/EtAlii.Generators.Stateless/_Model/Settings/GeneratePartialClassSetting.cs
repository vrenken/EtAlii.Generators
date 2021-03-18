namespace EtAlii.Generators.Stateless
{
    public class GeneratePartialClassSetting : Setting
    {
        public bool Value { get; }

        public GeneratePartialClassSetting(bool value)
        {
            Value = value;
        }
    }
}
