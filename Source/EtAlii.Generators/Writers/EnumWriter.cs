// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    public class EnumWriter<TInstance>
    {
        /// <summary>
        /// Write an enum.
        /// </summary>
        public void Write(WriteContext<TInstance> context, string[] comments, string name, string[] values)
        {
            foreach (var comment in comments)
            {
                context.Writer.WriteLine($"// {comment}");
            }
            context.Writer.WriteLine($"protected enum {name}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            foreach (var value in values)
            {
                context.Writer.WriteLine($"{value},");
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
