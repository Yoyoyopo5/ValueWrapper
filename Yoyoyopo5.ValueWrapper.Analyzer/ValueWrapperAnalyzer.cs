using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ValueWrapperAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
        ValueWrapperDiagnostics.PartialModifierRequiredWarning,
        ValueWrapperDiagnostics.PartialParentTypesRequiredWarning,
        ValueWrapperDiagnostics.JsonConstructionMethodMissingWarning,
        ValueWrapperDiagnostics.WronglyTypedValueProperty
    );

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSymbolAction(
            action: (context) =>
            {
                CancellationToken ct = context.CancellationToken;

                if (context.Compilation.GetTypeByMetadataName(ValueWrapperConstants.WRAPPER_ATTRIBUTE_NAME)
                    is not INamedTypeSymbol wrapperAttributeSymbol)
                    return;

                ct.ThrowIfCancellationRequested();
                if (((INamedTypeSymbol)context.Symbol).AsValueWrapperSymbol(wrapperAttributeSymbol) is not ValueWrapperSymbol wrapper)
                    return;

                wrapper
                    .AnalyzePartial(context.ReportDiagnostic, ct)
                    .AnalyzePartialParents(context.ReportDiagnostic, ct)
                    .AnalyzeValueProperty(context.ReportDiagnostic, ct)
                    .AnalyzeJsonConstruction(context.ReportDiagnostic, ct);
            },
            symbolKinds: ImmutableArray.Create(SymbolKind.NamedType)
            );
    }
}
