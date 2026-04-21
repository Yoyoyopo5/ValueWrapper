using Microsoft.CodeAnalysis;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains basic information about a specific containing (parent) type.
/// </summary>
public readonly record struct ParentTypeDefinition
{
    /// <summary>
    /// Get a <see cref="ParentTypeDefinition"/> from a parent <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="parentTypeSymbol">The parent type to analyze.</param>
    /// <param name="ct" />
    /// <returns>Parent type info about the <paramref name="parentTypeSymbol"/>.</returns>
    public static ParentTypeDefinition FromTypeSymbol(INamedTypeSymbol parentTypeSymbol, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return new()
        {
            Name = parentTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            PartialDeclaration = parentTypeSymbol.ToPartialTypeDeclarationString(),
            IsPartial = parentTypeSymbol.IsPartial()
        };
    }
    /// <summary>
    /// The name of the type, in <see cref="SymbolDisplayFormat.MinimallyQualifiedFormat"/>.
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// The declaration string needed to extend the type partially.
    /// </summary>
    public required string PartialDeclaration { get; init; }
    /// <summary>
    /// Indicates whether the parent type is marked <see langword="partial"/>.
    /// </summary>
    public required bool IsPartial { get; init; }
}
