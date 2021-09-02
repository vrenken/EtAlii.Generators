// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using EtAlii.Generators.PlantUml;

    public class StatelessMachineLifetime : IStateMachineLifetime
    {
        public string BeginStateName { get; } = "_Begin";
        public string EndStateName { get; } = "_End";
    }
}
