namespace EtAlii.Generators.Stateless
{
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    public partial class SourceGenerator
    {
        private void WriteNamespace(WriteContext context)
        {
            context.Writer.WriteLine($"// Remark: this file was auto-generated based on '{context.OriginalFileName}'.");
            context.Writer.WriteLine("// Any changes will be overwritten the next time the file is generated.");
            context.Writer.WriteLine($"namespace {context.StateMachine.Namespace}");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;
            context.Writer.WriteLine("using System;");
            context.Writer.WriteLine("using System.Threading.Tasks;");
            context.Writer.WriteLine("using Stateless;");

            foreach (var @using in context.StateMachine.Usings)
            {
                context.Writer.WriteLine($"using {@using};");
            }

            context.Writer.WriteLine();
            WriteClass(context);
            context.Writer.WriteLine();
            WriteBaseClass(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteClass(WriteContext context)
        {
            var prefix = context.StateMachine.GeneratePartialClass ? "partial" : "";
            context.Writer.WriteLine($"public {prefix} class {context.StateMachine.Class} : {context.StateMachine.Class}Base");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }


        private void WriteBaseClass(WriteContext context)
        {
            context.Writer.WriteLine("/// <summary>");
            context.Writer.WriteLine($"/// This is the base class for the state machine as defined in '{context.OriginalFileName}'.");
            context.Writer.WriteLine("/// Inherit the class and override the transition methods to define the necessary business behavior.");
            context.Writer.WriteLine("/// The transitions can then be triggered by calling the corresponding trigger methods.");
            context.Writer.WriteLine("/// </summary>");
            context.Writer.WriteLine($"public abstract class {context.StateMachine.Class}Base");
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
            context.Writer.WriteLine($"protected {context.StateMachine.Class}Base()");
            context.Writer.WriteLine("{");
            context.Writer.Indent += 1;

            WriteStateMachineInstantiation(context);

            context.Writer.Indent -= 1;
            context.Writer.WriteLine("}");
        }

        private void WriteStateMachineInstantiation(WriteContext context)
        {
            var startStates = context.StateMachine.StateFragments
                .OfType<StateTransition>()
                .Where(t => t.From == "None")
                .ToArray();
            if (startStates.Length == 0)
            {
                var startStatesAsString = string.Join(", ", startStates.Select(s => s.To));
                var location = Location.Create(context.OriginalFileName, new TextSpan(), new LinePositionSpan());
                var diagnostic = Diagnostic.Create(_noStartStatesDefinedRule, location, startStatesAsString);
                context.Diagnostics.Add(diagnostic);
            }
            else
            {
                context.Writer.WriteLine("// Time to create a new state machine instance.");
                context.Writer.WriteLine($"_stateMachine = new {StateMachineType}(State.None);");
                context.Writer.WriteLine();

                WriteTriggerConstructions(context);

                WriteStateConstructions(context);
            }
        }
    }
}
