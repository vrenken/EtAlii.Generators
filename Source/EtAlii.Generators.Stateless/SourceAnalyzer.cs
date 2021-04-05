namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SourceAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.CreateRange(AnalyzerRule.AllRules);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.RegisterCompilationStartAction(ActivateAnalyzerOnlyWhenNeeded);
        }

        private void Analyze(SymbolAnalysisContext context)
        {
            if (context.Symbol is INamedTypeSymbol namedTypeSymbol)
            {
                // Find the Stateless state machine type.
                var statelessStateMachineSymbol = GetStatelessStateMachineSymbol(context.Compilation);

                var baseType = namedTypeSymbol.BaseType;
                if (baseType != null)
                {
                    var hasStateMachineMember = baseType
                        .GetMembers()
                        .Where(m => m.Name == "_stateMachine")
                        .Cast<IFieldSymbol>()
                        .Where(m => m.IsReadOnly)
                        .Any(m => SymbolEqualityComparer.Default.Equals(m.Type.OriginalDefinition, statelessStateMachineSymbol));

                    if (hasStateMachineMember)
                    {
                        CheckAllVirtualMembersAreImplemented(context, baseType, namedTypeSymbol);
                    }
                }
            }
        }

        private void CheckAllVirtualMembersAreImplemented(SymbolAnalysisContext context, INamedTypeSymbol baseType, INamedTypeSymbol inheritingType)
        {
            var virtualMethods = baseType
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.IsVirtual)
                .ToArray();

            var implementedMethods = inheritingType
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.IsOverride)
                .ToArray();

            var notImplementedMethods = virtualMethods
                .Where(vm => implementedMethods.All(im => !SymbolEqualityComparer.Default.Equals(im.OverriddenMethod, vm)))
                .ToArray();

            foreach (var notImplementedMethod in notImplementedMethods)
            {
                var isVirtualStartMethod = notImplementedMethod.Name.StartsWith($"On{StatelessWriter.BeginStateName}");
                var isVirtualEndMethod = notImplementedMethod.Name.StartsWith($"On{StatelessWriter.EndStateName}");

                if (!isVirtualStartMethod && !isVirtualEndMethod)
                {
                    var properties = new Dictionary<string, string>
                    {
                        { "TargetClassName", inheritingType.Name },
                        { "MissingMethodName", notImplementedMethod.Name },
                        { "MethodParameterNames", string.Join("|",notImplementedMethod.Parameters.Select(p => p.Name)) },
                        { "MethodParameterTypes", string.Join("|",notImplementedMethod.Parameters.Select(p => context.Compilation.GetSpecialType(p.Type.SpecialType).Name)) },
                        { "MethodReturnType", notImplementedMethod.ReturnsVoid ? "void" : notImplementedMethod.ReturnType.Name }
                    };
                    var diagnostic = Diagnostic.Create(AnalyzerRule.MethodNotImplemented, inheritingType.Locations.First(), inheritingType.Locations, properties.ToImmutableDictionary(), inheritingType.Name, notImplementedMethod.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private void ActivateAnalyzerOnlyWhenNeeded(CompilationStartAnalysisContext context)
        {
            // Find the Stateless state machine type.
            var statelessStateMachineSymbol = GetStatelessStateMachineSymbol(context.Compilation);

            if (statelessStateMachineSymbol != null)
            {
                // register the analyzer on named type symbols
                context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
            }
        }

        private INamedTypeSymbol GetStatelessStateMachineSymbol(Compilation compilation)
        {
            return compilation.GetTypeByMetadataName("Stateless.StateMachine`2");
        }
    }
}
