namespace EtAlii.Generators.EntityFrameworkCore
{
    public class ClassNameSetting : Setting
    {
        public string Value { get; }

        public ClassNameSetting(string value)
        {
            Value = value;
        }
    }
}
