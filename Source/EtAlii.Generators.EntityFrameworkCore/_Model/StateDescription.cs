namespace EtAlii.Generators.EntityFrameworkCore
{
    public class StateDescription : StateFragment
    {
        public string State { get; }
        public string Text { get; }

        public StateDescription(string state, string text)
        {
            State = state;
            Text = text;
        }
    }
}
