// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators
{
    using System.CodeDom.Compiler;

    /// <summary>
    /// Use this class to transport actionable instances and data through the callstack of write methods.
    /// </summary>
    public class WriteContext<T>
    {
        /// <summary>
        /// The writer. Everything written using this instance finds its way into a source file.
        /// </summary>
        public IndentedTextWriter Writer { get; }

        /// <summary>
        /// THe original file name (i.e. the name of the original .puml file).
        /// </summary>
        public string OriginalFileName { get; }

        /// <summary>
        /// The parsed instance.
        /// </summary>
        public T Instance { get; }

        public NamespaceDetails NamespaceDetails { get; }

        public WriteContext(
            IndentedTextWriter writer,
            string originalFileName,
            T instance,
            NamespaceDetails namespaceDetails)
        {
            Writer = writer;
            OriginalFileName = originalFileName;
            Instance = instance;
            NamespaceDetails = namespaceDetails;
        }
    }
}
