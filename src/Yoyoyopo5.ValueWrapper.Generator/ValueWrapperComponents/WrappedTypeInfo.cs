using Microsoft.CodeAnalysis;
using System;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains information about a wrapped type symbol.
/// </summary>
public readonly record struct WrappedTypeInfo
{
    private static readonly SymbolDisplayFormat RealFullyQualifiedFormat = // string => global::System.String
        SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
        SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions &
        ~SymbolDisplayMiscellaneousOptions.UseSpecialTypes
        );
    /// <summary>
    /// Create a <see cref="WrappedTypeInfo"/> from a specific wrapped <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="symbol">The wrapped type.</param>
    /// <param name="ct" />
    /// <returns>
    /// A <see cref="WrappedTypeInfo"/> with info about a specific wrapped <see cref="INamedTypeSymbol"/>.
    /// </returns>
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
    /// <summary>
    /// The fully-qualified name of the type, including the global prefix.
    /// </summary>
    /// <remarks>
    /// This also applies to special types (e.g. <see langword="string"/>).
    /// </remarks>
    public required string FullyQualifiedName { get; init; }
    /// <summary>
    /// Indicates whether the type is <see cref="Nullable{T}"/> (value types)
    /// or is annotated as a nullable reference type.
    /// </summary>
    public required bool IsNullable { get; init; }
    /// <summary>
    /// Indicates whether the type is a value type.
    /// </summary>
    public required bool IsValueType { get; init; }
}
