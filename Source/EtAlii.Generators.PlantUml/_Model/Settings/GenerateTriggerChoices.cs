namespace EtAlii.Generators.PlantUml
{
    public class GenerateTriggerChoices : Setting
    {
        public bool Value { get; }

        public GenerateTriggerChoices(bool value)
        {
            Value = value;
        }
    }
}
