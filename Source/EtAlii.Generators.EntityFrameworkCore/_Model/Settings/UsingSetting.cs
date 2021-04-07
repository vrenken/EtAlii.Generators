namespace EtAlii.Generators.EntityFrameworkCore
{
    public class UsingSetting : Setting
    {
        public string Value { get; }

        public UsingSetting(string value)
        {
            Value = value;
        }
    }
}
