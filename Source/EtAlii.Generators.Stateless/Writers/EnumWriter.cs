namespace EtAlii.Generators.Stateless
{
    public class EnumWriter
    {
        /// <summary>
        /// Write the state machine internal State enum.
        /// </summary>
        public void WriteStateEnum(WriteContext context)
        {
            context.Writer.WriteLine("// Of course each state machine needs a set of states.");
            context.Writer.WriteLine("protected enum State");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            foreach (var state in context.AllStates)
            {
                context.Writer.WriteLine($"{state},");
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        /// <summary>
        /// Write the state machine internal Trigger enum.
        /// </summary>
        public void WriteTriggerEnum(WriteContext context)
        {
            context.Writer.WriteLine("// And all state machine need something that trigger them.");
            context.Writer.WriteLine("protected enum Trigger");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            foreach (var trigger in context.AllTriggers)
            {
                context.Writer.WriteLine($"{trigger},");
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
