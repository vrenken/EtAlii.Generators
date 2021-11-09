namespace EtAlii.Generators.PlantUml
{
    using Serilog;

    public partial class StateFragmentHelper
    {
        private readonly IStateMachineLifetime _lifetime;
        private readonly ILogger _log = Log.ForContext<StateFragmentHelper>();

        public StateFragmentHelper(IStateMachineLifetime lifetime)
        {
            _lifetime = lifetime;
        }

    }
}
