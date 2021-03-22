namespace EtAlii.Generators.Stateless
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// A code generator able to create Stateless source code from PlantUML diagrams.
    /// </summary>
    /// <remarks>
    /// The concepts below are not supported yet:
    /// - Nested states
    /// - Global transitions
    /// - Same named triggers with differently named parameters.
    /// </remarks>
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        public const string StateMachineType = "global::Stateless.StateMachine<State, Trigger>";
        public const string BeginStateName = "_Begin";
        public const string EndStateName = "_End";

        private readonly NamespaceWriter _namespaceWriter;
        private readonly StatelessPlantUmlParser _plantUmlParser;
        private readonly StatelessPlantUmlValidator _plantUmlValidator;
        public SourceGenerator()
        {
            // Layman's dependency injection:
            // No need to introduce a whole new package here as it'll only make the analyzer more bloated.
            // For now the simple composition below also works absolutely fine.
            var parameterConverter = new ParameterConverter();
            var transitionConverter = new TransitionConverter(parameterConverter);
            var enumWriter = new EnumWriter();
            var methodWriter = new MethodWriter(parameterConverter, transitionConverter);
            var fieldWriter = new FieldWriter(parameterConverter, transitionConverter);
            var instantiationWriter = new InstantiationWriter(parameterConverter, transitionConverter);
            var classWriter = new ClassWriter(enumWriter, fieldWriter, methodWriter, instantiationWriter);
            _namespaceWriter = new NamespaceWriter(classWriter);

            _plantUmlParser = new StatelessPlantUmlParser();
            _plantUmlValidator = new StatelessPlantUmlValidator();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // For testing and troubleshooting we'll use a simple list of strings that will be dumped to a file
            // at the end of the process.
            var log = new List<string>();

            // For actual troubleshooting of the source diagrams we use the Roslyn specific Diagnostics pattern.
            var diagnostics = new List<Diagnostic>();

            // This code generator is only able to understand and parse PlantUml files.
            // Because of that we ignore everything except files with the .puml extension.
            var plantUmlFiles = context.AdditionalFiles
                .Where(file => Path.GetExtension(file.Path).Equals(".puml", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach(var file in plantUmlFiles)
            {
                // First thing to do is parse the .puml file and build an in-memory model.
                if (_plantUmlParser.TryParsePlantUml(file, log, out var stateMachine, out var parseDiagnostics))
                {
                    try
                    {
                        var originalFileName = Path.GetFileName(file.Path);

                        // If there is no classname defined in the diagram we'll need to come up with one ourselves.
                        if (!stateMachine.Settings.OfType<ClassNameSetting>().Any())
                        {
                            // Let's use a C# safe subset of the characters in the filename.
                            var classNameFromFileName = Regex.Replace(Path.GetFileNameWithoutExtension(originalFileName), "[^a-zA-Z0-9_]", "");
                            stateMachine.AddSettings(new ClassNameSetting(classNameFromFileName));
                        }

                        var fileName = Path.ChangeExtension(originalFileName, "Generated.cs");

                        using var stringWriter = new StringWriter();
                        using var writer = new IndentedTextWriter(stringWriter);
                        var writeContext = new WriteContextFactory().Create(writer, originalFileName, log, stateMachine);

                        _plantUmlValidator.Validate(writeContext, diagnostics);

                        _namespaceWriter.Write(writeContext);

                        var content = stringWriter.ToString();
                        context.AddSource(fileName, content);
                    }
                    catch (Exception e)
                    {
                        log.Add($"File writing throws exception: {e.Message}");
                        log.Add($"{e.StackTrace}");

                        var location = Location.Create(file.Path, new TextSpan(), new LinePositionSpan());
                        var diagnostic = Diagnostic.Create(DiagnosticRule.PlantUmlStateMachineProcessingThrowsException, location, e.Message, e.StackTrace);
                        diagnostics.Add(diagnostic);
                    }
                }

                diagnostics.AddRange(parseDiagnostics);

                // Report any error/info, warning etc.
                foreach (var diagnostic in diagnostics)
                {
                    context.ReportDiagnostic(diagnostic);
                }
            }

            log = log.SelectMany(l => l.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.None)).ToList();
            log = log.Select(l => $"// {l}").ToList();
            context.AddSource( "Log.txt", string.Join("\n",log));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // Nothing to initialize.
        }
    }
}
