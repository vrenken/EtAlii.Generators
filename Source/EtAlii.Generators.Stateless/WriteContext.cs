namespace EtAlii.Generators.Stateless
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    /// <summary>
    /// Use this class to transport actionable instances and data through the callstack of write methods.
    /// </summary>
    internal class WriteContext
    {
        /// <summary>
        /// The writer. Everything written using this instance finds its way into a source file.
        /// </summary>
        public IndentedTextWriter Writer { get; }

        /// <summary>
        /// THe original file name (i.e. the name of the original .puml file).
        /// </summary>
        public string OriginalFileName { get; }
        public List<string> Log { get; }
        public List<Diagnostic> Diagnostics { get; }

        /// <summary>
        /// The parsed state machine.
        /// </summary>
        public StateMachine StateMachine { get; }

        public StateTransition[] UniqueParameterTransitions { get; }
        public StateTransition[] AllTransitions { get; }
        public string[] AllStates { get; }
        public string[] AllTriggers { get; }

        public WriteContext(
            IndentedTextWriter writer,
            string originalFileName,
            List<string> log,
            List<Diagnostic> diagnostics,
            StateMachine stateMachine,
            string[] allStates,
            string[] allTriggers,
            StateTransition[] allTransitions,
            StateTransition[] uniqueParameterTransitions)
        {
            Writer = writer;
            OriginalFileName = originalFileName;
            Log = log;
            Diagnostics = diagnostics;
            StateMachine = stateMachine;
            AllStates = allStates;
            AllTriggers = allTriggers;
            AllTransitions = allTransitions;
            UniqueParameterTransitions = uniqueParameterTransitions;
        }
    }
}
