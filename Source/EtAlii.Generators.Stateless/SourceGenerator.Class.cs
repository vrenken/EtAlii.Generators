namespace EtAlii.Generators.Stateless
{
    public partial class SourceGenerator
    {
        private void WriteClass(WriteContext context)
        {
            var prefix = context.StateMachine.GeneratePartialClass ? "abstract partial" : "abstract";

            context.Writer.WriteLine("/// <summary>");
            context.Writer.WriteLine($"/// This is the base class for the state machine as defined in '{context.OriginalFileName}'.");
            context.Writer.WriteLine("/// Inherit the class and override the transition methods to define the necessary business behavior.");
            context.Writer.WriteLine("/// The transitions can then be triggered by calling the corresponding trigger methods.");
            context.Writer.WriteLine("/// </summary>");
            context.Writer.WriteLine($"public {prefix} class {context.StateMachine.ClassName}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            context.Writer.WriteLine($"protected {StateMachineType} StateMachine => _stateMachine;");
            context.Writer.WriteLine($"private readonly {StateMachineType} _stateMachine;");
            context.Writer.WriteLine();

            WriteAllTriggerMembers(context);
            context.Writer.WriteLine();

            WriteConstructor(context);
            context.Writer.WriteLine();

            WriteTriggerMethods(context);
            context.Writer.WriteLine();

            WriteStateEnum(context);
            context.Writer.WriteLine();

            WriteTriggerEnum(context);
            context.Writer.WriteLine();

            WriteTransitionMethods(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteConstructor(WriteContext context)
        {
            context.Writer.WriteLine($"protected {context.StateMachine.ClassName}()");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            WriteStateMachineInstantiation(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
