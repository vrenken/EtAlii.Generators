namespace EtAlii.Generators.EntityFrameworkCore
{
    public class DbContextNameSetting : Setting
    {
        public string Value { get; }

        public DbContextNameSetting(string value)
        {
            Value = value;
        }
    }
}
