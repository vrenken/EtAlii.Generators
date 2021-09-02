namespace EtAlii.Generators.Stateless
{
    using EtAlii.Generators.PlantUml;

    public class ClassWriter : IWriter<StateMachine>
    {
        private readonly EnumWriter<StateMachine> _enumWriter;
        private readonly FieldWriter _fieldWriter;
        private readonly MethodWriter _methodWriter;
        private readonly EventArgsWriter _eventArgsWriter;
        private readonly InstantiationWriter _instantiationWriter;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public ClassWriter(
            EnumWriter<StateMachine> enumWriter,
            FieldWriter fieldWriter,
            MethodWriter methodWriter,
            EventArgsWriter eventArgsWriter,
            InstantiationWriter instantiationWriter,
            StateFragmentHelper stateFragmentHelper)
        {
            _enumWriter = enumWriter;
            _fieldWriter = fieldWriter;
            _methodWriter = methodWriter;
            _eventArgsWriter = eventArgsWriter;
            _instantiationWriter = instantiationWriter;
            _stateFragmentHelper = stateFragmentHelper;
        }

        public void Write(WriteContext<StateMachine> context)
        {
            var prefix = context.Instance.GeneratePartialClass ? "partial" : "abstract";
            var action1 = context.Instance.GeneratePartialClass ? "Inherit" : "Add another partial for";
            var action2 = context.Instance.GeneratePartialClass ? "override" : "implement";

            context.Writer.WriteLine("/// <summary>");
            context.Writer.WriteLine($"/// This is the {prefix} class for the state machine as defined in '{context.OriginalFileName}'.");
            context.Writer.WriteLine($"/// {action1} the class and {action2} the transition methods to define the necessary business behavior.");
            context.Writer.WriteLine("/// The transitions can then be triggered by calling the corresponding trigger methods.");
            context.Writer.WriteLine("/// </summary>");
            context.Writer.WriteLine($"public {prefix} class {context.Instance.ClassName}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            context.Writer.WriteLine($"protected {StatelessWriter.StateMachineType} StateMachine => _stateMachine;");
            context.Writer.WriteLine($"private readonly {StatelessWriter.StateMachineType} _stateMachine;");
            context.Writer.WriteLine();

            _fieldWriter.WriteAllTriggerFields(context);
            context.Writer.WriteLine();

            WriteConstructor(context);
            context.Writer.WriteLine();

            _methodWriter.WriteTriggerMethods(context);
            context.Writer.WriteLine();

            _eventArgsWriter.WriteEventArgs(context);
            context.Writer.WriteLine();

            var allStates = _stateFragmentHelper.GetAllStates(context.Instance.StateFragments);
            _enumWriter.Write(context, new []{ "Of course each state machine needs a set of states."}, "State", allStates);
            context.Writer.WriteLine();

            var allTriggers = _stateFragmentHelper.GetAllTriggers(context.Instance.StateFragments);
            _enumWriter.Write(context, new []{ "And all state machine need something that trigger them."}, "Trigger", allTriggers);
            context.Writer.WriteLine();

            _methodWriter.WriteTransitionMethods(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteConstructor(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine($"protected {context.Instance.ClassName}()");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            _instantiationWriter.WriteStateMachineInstantiation(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }
    }
}
