﻿namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class ClassWriter : IWriter<StateMachine>
    {
        private readonly EnumWriter<StateMachine> _enumWriter;
        private readonly FieldWriter _fieldWriter;
        private readonly MethodWriter _methodWriter;
        private readonly TriggerClassWriter _triggerClassWriter;
        private readonly InstantiationWriter _instantiationWriter;

        public ClassWriter(
            EnumWriter<StateMachine> enumWriter,
            FieldWriter fieldWriter,
            MethodWriter methodWriter,
            TriggerClassWriter triggerClassWriter,
            InstantiationWriter instantiationWriter)
        {
            _enumWriter = enumWriter;
            _fieldWriter = fieldWriter;
            _methodWriter = methodWriter;
            _triggerClassWriter = triggerClassWriter;
            _instantiationWriter = instantiationWriter;
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

            context.Writer.WriteLine($"protected {context.Instance.ClassName}.State _state;");

            context.Writer.WriteLine($"private readonly Queue<Action> _transactions = new Queue<Action>();");
            context.Writer.WriteLine();

            WriteRunOrQueueTransition(context);

            _methodWriter.WriteTriggerMethods(context);
            context.Writer.WriteLine();

            _triggerClassWriter.WriteTriggerClasses(context);
            context.Writer.WriteLine();

            var allStates = StateFragment.GetAllStates(context.Instance.StateFragments);
            _enumWriter.Write(context, new []{ "Of course each state machine needs a set of states."}, "State", allStates);
            context.Writer.WriteLine();

            // var allTriggers = StateFragment.GetAllTriggers(context.Instance.StateFragments);
            // _enumWriter.Write(context, new []{ "And all state machine need something that trigger them."}, "Trigger", allTriggers);
            // context.Writer.WriteLine();

            _methodWriter.WriteTransitionMethods(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteRunOrQueueTransition(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine($"private void RunOrQueueTransition(Action transition)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"var deQueue = _transactions.Count > 0;");
            context.Writer.WriteLine($"_transactions.Enqueue(transition);");
            context.Writer.WriteLine($"if (deQueue)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"while(_transactions.TryDequeue(out var queuedTransaction))");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"queuedTransaction();");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
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
