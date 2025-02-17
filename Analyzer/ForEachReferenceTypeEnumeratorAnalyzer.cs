using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ForEachReferenceTypeEnumeratorAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get => ImmutableArray.Create(Rule);
        }

        private const string Id = "SMP0001";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.ForEachReferenceTypeEnumerator_Title), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.ForEachReferenceTypeEnumerator_MessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.ForEachReferenceTypeEnumerator_Description), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));

#pragma warning disable RS2008
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(Id, Title, MessageFormat, "Usage", DiagnosticSeverity.Info, true, Description);
#pragma warning restore RS2008

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ForEachStatement);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is ForEachStatementSyntax forEachSyntax))
            {
                return;
            }

            var expression = forEachSyntax.Expression;
            var returnType = context.SemanticModel.GetTypeInfo(expression).Type;

            var statementInfo = context.SemanticModel.GetForEachStatementInfo(forEachSyntax);
            if (!statementInfo.GetEnumeratorMethod.ReturnType.IsReferenceType || returnType.TypeKind == TypeKind.Array)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, forEachSyntax.Expression.GetLocation()));
        }
    }
}
