namespace EtAlii.Generators.Stateless
{
    using System.CodeDom.Compiler;

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

        /// <summary>
        /// The parsed state machine.
        /// </summary>
        public StateMachine StateMachine { get; }

        public Transition[] UniqueParameterTransitions { get; }
        public Transition[] AllTransitions { get; }
        public string[] AllStates { get; }
        public string[] AllTriggers { get; }

        public WriteContext(
            IndentedTextWriter writer,
            string originalFileName,
            StateMachine stateMachine,
            string[] allStates,
            string[] allTriggers,
            Transition[] allTransitions,
            Transition[] uniqueParameterTransitions)
        {
            Writer = writer;
            OriginalFileName = originalFileName;
            StateMachine = stateMachine;
            AllStates = allStates;
            AllTriggers = allTriggers;
            AllTransitions = allTransitions;
            UniqueParameterTransitions = uniqueParameterTransitions;
        }
    }
}
