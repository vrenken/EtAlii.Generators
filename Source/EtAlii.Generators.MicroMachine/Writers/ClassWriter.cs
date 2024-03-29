﻿namespace EtAlii.Generators.MicroMachine
{
    using System.Linq;
    using EtAlii.Generators.PlantUml;
    using Serilog;

    public class ClassWriter : IWriter<StateMachine>
    {
        private readonly EnumWriter<StateMachine> _enumWriter;
        private readonly TransitionMethodWriter _transitionMethodWriter;
        private readonly TriggerMethodWriter _triggerMethodWriter;
        private readonly TriggerClassWriter _triggerClassWriter;
        private readonly TransitionClassWriter _transitionClassWriter;
        private readonly StateFragmentHelper _stateFragmentHelper;
        private readonly ParameterConverter _parameterConverter;
        private readonly ChoicesWriter _choicesWriter;
        private readonly ILogger _log = Log.ForContext<ClassWriter>();

        public ClassWriter(EnumWriter<StateMachine> enumWriter,
            TransitionMethodWriter transitionMethodWriter,
            TriggerMethodWriter triggerMethodWriter,
            TriggerClassWriter triggerClassWriter,
            TransitionClassWriter transitionClassWriter,
            StateFragmentHelper stateFragmentHelper,
            ParameterConverter parameterConverter,
            ChoicesWriter choicesWriter)
        {
            _enumWriter = enumWriter;
            _transitionMethodWriter = transitionMethodWriter;
            _triggerMethodWriter = triggerMethodWriter;
            _triggerClassWriter = triggerClassWriter;
            _transitionClassWriter = transitionClassWriter;
            _stateFragmentHelper = stateFragmentHelper;
            _parameterConverter = parameterConverter;
            _choicesWriter = choicesWriter;
        }

        public void Write(WriteContext<StateMachine> context)
        {
            _log.Information("Writing class {ClassName}", context.Instance.ClassName);

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

            WriteFieldsAndProperties(context);
            context.Writer.WriteLine();

            WriteConstuctor(context);
            WriteRunOrQueueTransition(context);

            _triggerMethodWriter.WriteTriggerMethods(context);
            context.Writer.WriteLine();

            _choicesWriter.WriteChoices(context);
            context.Writer.WriteLine();

            _transitionClassWriter.WriteTransitionClasses(context);
            context.Writer.WriteLine();

            _triggerClassWriter.WriteTriggerClasses(context);
            context.Writer.WriteLine();

            var allStates = context.Instance.SequentialStates
                .Select(s => s.Name)
                .ToArray();
            _enumWriter.Write(context, new []{ "Of course each state machine needs a set of states."}, "State", allStates);
            context.Writer.WriteLine();

            _transitionMethodWriter.WriteTransitionMethods(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteFieldsAndProperties(WriteContext<StateMachine> context)
        {
            _log.Information("Writing fields and properties for {ClassName}", context.Instance.ClassName);

            context.Writer.WriteLine($"protected {context.Instance.ClassName}.State _state;");
            context.Writer.WriteLine($"private bool _queueTransitions;");
            context.Writer.WriteLine($"private readonly Queue<Transition> _transactions = new Queue<Transition>();");

            if (context.Instance.GenerateTriggerChoices)
            {
                foreach (var state in context.Instance.SequentialStates)
                {
                    context.Writer.WriteLine($"private readonly {context.Instance.ClassName}.{state.Name}Choices _{_parameterConverter.ToCamelCase(state.Name)}Choices;");
                }
            }
            context.Writer.WriteLine();
        }

        private void WriteConstuctor(WriteContext<StateMachine> context)
        {
            _log.Information("Writing constructor for {ClassName}", context.Instance.ClassName);

            context.Writer.WriteLine($"public {context.Instance.ClassName}()");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            if (context.Instance.GenerateTriggerChoices)
            {
                foreach (var state in context.Instance.SequentialStates)
                {
                    context.Writer.WriteLine($"_{_parameterConverter.ToCamelCase(state.Name)}Choices = new {context.Instance.ClassName}.{state.Name}Choices(this);");
                }
            }

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
            context.Writer.WriteLine();
        }

        private void WriteRunOrQueueTransition(WriteContext<StateMachine> context)
        {
            _log.Information("Writing run or queue transition method for {ClassName}", context.Instance.ClassName);

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
