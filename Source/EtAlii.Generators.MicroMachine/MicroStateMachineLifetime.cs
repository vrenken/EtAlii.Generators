// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class MicroStateMachineLifetime : StateMachineLifetimeBase, IStateMachineLifetime
    {
        public string BeginStateName => "_Idle";
        public string EndStateName => "_Idle";
    }
}
