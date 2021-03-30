namespace EtAlii.Generators.GraphQL.Client
{
    public class ClassWriter
    {
        private readonly EnumWriter _enumWriter;
        private readonly FieldWriter _fieldWriter;
        private readonly MethodWriter _methodWriter;
        private readonly InstantiationWriter _instantiationWriter;

        public ClassWriter(
            EnumWriter enumWriter,
            FieldWriter fieldWriter,
            MethodWriter methodWriter,
            InstantiationWriter instantiationWriter)
        {
            _enumWriter = enumWriter;
            _fieldWriter = fieldWriter;
            _methodWriter = methodWriter;
            _instantiationWriter = instantiationWriter;
        }

        public void Write(WriteContext context)
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

            context.Writer.WriteLine($"protected {SourceGenerator.StateMachineType} StateMachine => _stateMachine;");
            context.Writer.WriteLine($"private readonly {SourceGenerator.StateMachineType} _stateMachine;");
            context.Writer.WriteLine();

            _fieldWriter.WriteAllTriggerFields(context);
            context.Writer.WriteLine();

            WriteConstructor(context);
            context.Writer.WriteLine();

            _methodWriter.WriteTriggerMethods(context);
            context.Writer.WriteLine();

            _enumWriter.WriteStateEnum(context);
            context.Writer.WriteLine();

            _enumWriter.WriteTriggerEnum(context);
            context.Writer.WriteLine();

            _methodWriter.WriteTransitionMethods(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteConstructor(WriteContext context)
        {
            context.Writer.WriteLine($"protected {context.StateMachine.ClassName}()");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            _instantiationWriter.WriteStateMachineInstantiation(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
