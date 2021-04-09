// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    public class NamespaceDetails
    {
        public string Name { get; }
        public string[] Usings { get; }

        public NamespaceDetails(string name, string[] usings)
        {
            Name = name;
            Usings = usings;
        }
    }
}
