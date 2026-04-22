using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Analyzer.ValueWrapperComponents;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Analyzer;

internal static class ValueWrapperSymbolAnalyzerExtensions
{
    public static ValueWrapperSymbol AnalyzeStaticType(this ValueWrapperSymbol wrapperSymbol, Action<Diagnostic> report, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (wrapperSymbol.TypeSymbol.IsStatic)
            report(Diagnostic.Create(
                ValueWrapperDiagnostics.StaticWrapperTypeWarning,
                wrapperSymbol.TypeSymbol.Locations.FirstOrDefault(),
                wrapperSymbol.TypeSymbol.Name
                ));
        return wrapperSymbol;
    }

    public static ValueWrapperSymbol AnalyzePartial(this ValueWrapperSymbol wrapperSymbol, Action<Diagnostic> report, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (!wrapperSymbol.TypeSymbol.IsPartial())
            report(Diagnostic.Create(
                ValueWrapperDiagnostics.PartialModifierRequiredWarning,
                wrapperSymbol.TypeSymbol.Locations.FirstOrDefault(),
                wrapperSymbol.TypeSymbol.Name
                ));
        return wrapperSymbol;
    }

    public static ValueWrapperSymbol AnalyzePartialParents(this ValueWrapperSymbol wrapperSymbol, Action<Diagnostic> report, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (wrapperSymbol.TypeSymbol.GetContainingTypes().FirstOrDefault(parent => !parent.IsPartial()) is INamedTypeSymbol nonPartialParent)
            report(Diagnostic.Create(
                ValueWrapperDiagnostics.PartialParentTypesRequiredWarning,
                nonPartialParent.Locations.FirstOrDefault(),
                nonPartialParent.Name,
                wrapperSymbol.TypeSymbol.Name
                ));
        return wrapperSymbol;
    }

    public static ValueWrapperSymbol AnalyzeValueProperty(this ValueWrapperSymbol wrapperSymbol, Action<Diagnostic> report, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (wrapperSymbol.GetWrapperValuePropertyOrDefault() is { IsOfWrappedType: false })
            report(Diagnostic.Create(
                ValueWrapperDiagnostics.WronglyTypedValueProperty,
                wrapperSymbol.TypeSymbol.GetMembers("Value").First().Locations.FirstOrDefault(),
                wrapperSymbol.TypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                wrapperSymbol.WrappedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                ));
        return wrapperSymbol;
    }

    public static ValueWrapperSymbol AnalyzeRecordConstructor(this ValueWrapperSymbol wrapperSymbol, Action<Diagnostic> report, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (!wrapperSymbol.TypeSymbol.IsRecord)
            return wrapperSymbol;
        if (wrapperSymbol.TypeSymbol.InstanceConstructors.FirstOrDefault(c => c.IsPrimaryConstructor()) is not IMethodSymbol primary)
            return wrapperSymbol;
        if (primary.Parameters.Length > 0 && !primary.HasExactParametersOfType(wrapperSymbol.WrappedTypeSymbol))
            report(Diagnostic.Create(
                ValueWrapperDiagnostics.WronglyTypedValueProperty,
                primary.Locations.FirstOrDefault(),
                wrapperSymbol.TypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                wrapperSymbol.WrappedTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                ));
        return wrapperSymbol;
    }

    public static ValueWrapperSymbol AnalyzeJsonConstruction(this ValueWrapperSymbol wrapperSymbol, Action<Diagnostic> report, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (wrapperSymbol.GetWrapperStaticCreateMethodOrDefault() is not null)
            return wrapperSymbol;
        if (wrapperSymbol.TypeSymbol.InstanceConstructors.Any(c => c.Parameters.Length == 0)
            && (wrapperSymbol.GetWrapperValuePropertyOrDefault() is not ValuePropertyInfo valueProperty || valueProperty.Initializable))
            return wrapperSymbol; // initializable
        if (wrapperSymbol.GetWrapperConstructorOrDefault() is null)
            report(Diagnostic.Create(
                ValueWrapperDiagnostics.JsonConstructionMethodMissingWarning,
                wrapperSymbol.TypeSymbol.Locations.FirstOrDefault(),
                wrapperSymbol.TypeSymbol.Name
                ));
        return wrapperSymbol;
    }
}
