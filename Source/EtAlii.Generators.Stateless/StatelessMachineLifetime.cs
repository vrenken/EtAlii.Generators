// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using EtAlii.Generators.PlantUml;

    public class StatelessMachineLifetime : StateMachineLifetimeBase, IStateMachineLifetime
    {
        public string BeginStateName => "_Begin";
        public string EndStateName => "_End";
    }
}
