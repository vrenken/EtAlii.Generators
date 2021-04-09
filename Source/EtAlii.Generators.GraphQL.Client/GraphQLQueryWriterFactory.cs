// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.GraphQL.Client
{
    public class GraphQLQueryWriterFactory : IWriterFactory<object>, IWriter<object>
    {
        public IWriter<object> Create()
        {
            return this;
        }

        public void Write(WriteContext<object> context)
        {
            context.Writer.Write("Test");
        }
    }
}
