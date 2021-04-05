// Copyright (c) Peter Vrenken. All rights reserved. See https://github.com/vrenken/EtAlii.Generators for more information and the license.

namespace EtAlii.Generators.Stateless
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;

    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public class SourceFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get; } = ImmutableArray.Create(new[] {AnalyzerRule.MethodNotImplemented.Id});

        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                if (!diagnostic.IsSuppressed)
                {
                    var missingMethodName = diagnostic.Properties["MissingMethodName"];
                    var targetClassName = diagnostic.Properties["TargetClassName"];
                    var uniqueTitle = $"Add state machine method '{missingMethodName}' to class '{targetClassName}'";
                    var codeAction = CodeAction.Create(
                        uniqueTitle,
                        cancellationToken => GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        "Add state machine method");
                    context.RegisterCodeFix(codeAction, diagnostic);
                }
            }
            return Task.CompletedTask;
        }

        private async Task<Document> GetTransformedDocumentAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var targetClassName = diagnostic.Properties["TargetClassName"];

            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            var classDeclaration = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(cds => cds.Identifier.Text == targetClassName);
            if (classDeclaration == null) return document;

            var methodDeclaration = GetMethodDeclarationSyntax(diagnostic.Properties);
            var newClassDeclaration = classDeclaration.AddMembers(methodDeclaration);
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);
            var newDocument = document.WithSyntaxRoot(newRoot);
            return newDocument;
        }

        private MethodDeclarationSyntax GetMethodDeclarationSyntax(ImmutableDictionary<string, string> properties)
        {
            var missingMethodName = properties["MissingMethodName"];
            var methodParameterNames = properties["MethodParameterNames"]!.Split('|').ToArray();
            var methodParameterTypes = properties["MethodParameterTypes"]!.Split('|').ToArray();
            var methodReturnType = properties["MethodReturnType"];

            var syntax = SyntaxFactory.ParseStatement(@"throw new System.NotImplementedException();");
            var parameterList = SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(GetParametersList(methodParameterTypes, methodParameterNames)));
            var modifiers = new[]
            {
                SyntaxFactory.Token(SyntaxKind.ProtectedKeyword),
                SyntaxFactory.Token(SyntaxKind.OverrideKeyword)
            };

            return SyntaxFactory.MethodDeclaration(attributeLists: SyntaxFactory.List<AttributeListSyntax>(),
                    modifiers: SyntaxFactory.TokenList(modifiers),
                    returnType: SyntaxFactory.ParseTypeName(methodReturnType),
                    explicitInterfaceSpecifier: null!,
                    identifier: SyntaxFactory.Identifier(missingMethodName),
                    typeParameterList: null!,
                    parameterList: parameterList,
                    constraintClauses: SyntaxFactory.List<TypeParameterConstraintClauseSyntax>(),
                    body: SyntaxFactory.Block(syntax),
                    semicolonToken:  SyntaxFactory.Token(SyntaxKind.None))
                // Annotate that this node should be formatted
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        private IEnumerable<ParameterSyntax> GetParametersList(string[] parameterTypes, string[] parameterNames)
        {
            return parameterTypes
                .Select((t, i) => SyntaxFactory.Parameter(
                    attributeLists: SyntaxFactory.List<AttributeListSyntax>(),
                    modifiers: SyntaxFactory.TokenList(),
                    type: SyntaxFactory.ParseTypeName(t),
                    identifier: SyntaxFactory.Identifier(parameterNames[i]),
                    @default: null));
        }
    }
}
