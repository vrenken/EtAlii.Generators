// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.EntityFrameworkCore
{
    public class EntityNameSetting : Setting
    {
        public string Value { get; }

        public EntityNameSetting(string value)
        {
            Value = value;
        }
    }
}
