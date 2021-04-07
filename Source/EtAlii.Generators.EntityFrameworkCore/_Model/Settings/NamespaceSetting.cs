namespace EtAlii.Generators.EntityFrameworkCore
{
    public class NamespaceSetting : Setting
    {
        public string Value { get; }

        public NamespaceSetting(string value)
        {
            Value = value;
        }
    }
}
