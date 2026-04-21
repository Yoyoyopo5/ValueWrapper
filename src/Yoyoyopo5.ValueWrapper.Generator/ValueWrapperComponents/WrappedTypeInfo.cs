using Microsoft.CodeAnalysis;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct WrappedTypeInfo
{
    private static readonly SymbolDisplayFormat RealFullyQualifiedFormat = // string => global::System.String
        SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
        SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions &
        ~SymbolDisplayMiscellaneousOptions.UseSpecialTypes
        );
    public static WrappedTypeInfo FromTypeSymbol(INamedTypeSymbol symbol, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return new()
        {
            FullyQualifiedName = symbol.ToDisplayString(RealFullyQualifiedFormat) + ((!symbol.IsValueType && symbol.IsNullable()) ? "?" : string.Empty),
            IsNullable = symbol.IsNullable(),
            IsValueType = symbol.IsValueType
        };
    }
    public required string FullyQualifiedName { get; init; }
    public required bool IsNullable { get; init; }
    public required bool IsValueType { get; init; }
}
