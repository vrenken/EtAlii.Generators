namespace EtAlii.Generators.MicroMachine
{
    using EtAlii.Generators.PlantUml;

    public class StateMachineClassWriter : IWriter<StateMachine>
    {
        private readonly EnumWriter<StateMachine> _enumWriter;
        private readonly MethodWriter _methodWriter;
        private readonly TriggerClassWriter _triggerClassWriter;
        private readonly TransitionClassWriter _transitionClassWriter;
        private readonly StateFragmentHelper _stateFragmentHelper;

        public StateMachineClassWriter(EnumWriter<StateMachine> enumWriter,
            MethodWriter methodWriter,
            TriggerClassWriter triggerClassWriter,
            TransitionClassWriter transitionClassWriter,
            StateFragmentHelper stateFragmentHelper)
        {
            _enumWriter = enumWriter;
            _methodWriter = methodWriter;
            _triggerClassWriter = triggerClassWriter;
            _transitionClassWriter = transitionClassWriter;
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

            context.Writer.WriteLine($"protected {context.Instance.ClassName}.State _state;");
            context.Writer.WriteLine($"private bool _queueTransitions;");

            context.Writer.WriteLine($"private readonly Queue<Transition> _transactions = new Queue<Transition>();");
            context.Writer.WriteLine();

            WriteRunOrQueueTransition(context);

            _methodWriter.WriteTriggerMethods(context);
            context.Writer.WriteLine();

            _transitionClassWriter.WriteTransitionClasses(context);
            context.Writer.WriteLine();

            _triggerClassWriter.WriteTriggerClasses(context);
            context.Writer.WriteLine();

            var allStates = _stateFragmentHelper.GetAllStates(context.Instance.StateFragments);
            _enumWriter.Write(context, new []{ "Of course each state machine needs a set of states."}, "State", allStates);
            context.Writer.WriteLine();

            _methodWriter.WriteTransitionMethods(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteRunOrQueueTransition(WriteContext<StateMachine> context)
        {
            context.Writer.WriteLine($"private void RunOrQueueTransition(Transition transition)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("if(_queueTransitions)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"_transactions.Enqueue(transition);");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine("else");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("_queueTransitions = true;");
            context.Writer.WriteLine($"_transactions.Enqueue(transition);");
            context.Writer.WriteLine($"while(_transactions.TryDequeue(out var queuedTransaction))");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"((SyncTransition)queuedTransaction).Handler();");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine("_queueTransitions = false;");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();

            context.Writer.WriteLine($"private async Task RunOrQueueTransitionAsync(AsyncTransition transition)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("if(_queueTransitions)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine($"_transactions.Enqueue(transition);");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine("else");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("_queueTransitions = true;");
            context.Writer.WriteLine($"_transactions.Enqueue(transition);");
            context.Writer.WriteLine($"while(_transactions.TryDequeue(out var queuedTransaction))");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("switch(queuedTransaction)");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("case SyncTransition syncTransition: syncTransition.Handler(); break;");
            context.Writer.WriteLine("case AsyncTransition asyncTransition: await asyncTransition.Handler().ConfigureAwait(false); break;");
            context.Writer.WriteLine("default: throw new InvalidOperationException(\"This will never happen\");");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine("_queueTransitions = false;");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }
    }
}
