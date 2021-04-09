// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    public interface IWriter<TInstance>
    {
        void Write(WriteContext<TInstance> context);
    }
}
