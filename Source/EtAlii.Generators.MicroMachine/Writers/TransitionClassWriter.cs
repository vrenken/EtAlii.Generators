﻿namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class TransitionClassWriter
    {
        public void WriteTransitionClasses(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine("// The classes below represent the transitions as used by the state machine.");
            context.Writer.WriteLine();

            context.Writer.WriteLine("protected abstract class Transition");
            context.Writer.WriteLine("{");
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
            context.Writer.WriteLine("protected sealed class AsyncTransition : Transition");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("public Func<Task> Handler {get; init;}");
            context.Writer.WriteLine();
            context.Writer.WriteLine("public AsyncTransition(Func<Task> handler) => Handler = handler;");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
            context.Writer.WriteLine("protected sealed class SyncTransition : Transition");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("public Action Handler {get; init;}");
            context.Writer.WriteLine();
            context.Writer.WriteLine("public SyncTransition(Action handler) => Handler = handler;");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }
    }
}
