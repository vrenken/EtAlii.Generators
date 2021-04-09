// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.ML
{
    public class GraphQLQueryWriter : IWriter<object>
    {
        public void Write(WriteContext<object> context)
        {
            context.Writer.Write("Test");
        }
    }
}
