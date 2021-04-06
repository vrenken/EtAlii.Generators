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
                .Where(vm => !vm.Name.StartsWith($"On{StatelessWriter.BeginStateName}"))
                .Where(vm => !vm.Name.StartsWith($"On{StatelessWriter.EndStateName}"))
                .ToArray();

            foreach (var notImplementedMethod in notImplementedMethods)
            {
                var properties = new Dictionary<string, string>
                {
                    { "TargetClassName", inheritingType.Name },
                    { "MissingMethodName", notImplementedMethod.Name },
                    { "MethodParameterNames", string.Join("|",notImplementedMethod.Parameters.Select(p => p.Name)) },
                    { "MethodParameterTypes", string.Join("|",notImplementedMethod.Parameters.Select(GetParameterType)) },
                    { "MethodReturnType", notImplementedMethod.ReturnsVoid ? "void" : notImplementedMethod.ReturnType.Name }
                };
                var diagnostic = Diagnostic.Create(AnalyzerRule.MethodNotImplemented, inheritingType.Locations.First(), inheritingType.Locations, properties.ToImmutableDictionary(), inheritingType.Name, notImplementedMethod.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private string GetParameterType(IParameterSymbol parameterSymbol)
        {
            return parameterSymbol.Type.SpecialType switch
            {
                // Google did not help finding the right way to map these special types onto CLR keywords.
                // Therefore a simple mapping is the best we can get for now.
                SpecialType.System_Object => "object",
                SpecialType.System_Boolean => "bool",
                SpecialType.System_Char => "char",
                SpecialType.System_SByte => "sbyte",
                SpecialType.System_Byte => "byte",
                SpecialType.System_Int16 => "short",
                SpecialType.System_UInt16 => "ushort",
                SpecialType.System_Int32 => "int",
                SpecialType.System_UInt32 => "uint",
                SpecialType.System_Int64 => "long",
                SpecialType.System_UInt64 => "ulong",
                SpecialType.System_Decimal => "decimal",
                SpecialType.System_Single => "float",
                SpecialType.System_Double => "double",
                SpecialType.System_String => "string",
                _ => parameterSymbol.Type.Name,
            };
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
