using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains information about the wrapped <c>Value</c> property on a wrapper type.
/// </summary>
public readonly record struct ValuePropertyInfo
{
    /// <summary>
    /// Create a <see cref="ValuePropertyInfo"/> from a wrapper <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <remarks>
    /// This will include properties added implicitly by the <see langword="record"/> syntax with a primary constructor.
    /// </remarks>
    /// <param name="symbol">The wrapper type.</param>
    /// <param name="wrappedTypeSymbol">The wrapped type.</param>
    /// <param name="ct" />
    /// <returns>
    /// A <see cref="ValuePropertyInfo"/> with info on the <paramref name="symbol"/>'s <c>Value</c> property,
    /// or <see langword="null"/> if the property does not exist.
    /// </returns>
    public static ValuePropertyInfo? FromTypeSymbol(
        INamedTypeSymbol symbol,
        INamedTypeSymbol wrappedTypeSymbol,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (symbol.GetMembers("Value")
            .OfType<IPropertySymbol>()
            .FirstOrDefault() is not IPropertySymbol valueProperty)
            return null;
        ct.ThrowIfCancellationRequested();
        return new()
        {
            Name = valueProperty.Name,
            Initializable = valueProperty.SetMethod switch
            {
                { DeclaredAccessibility: Accessibility.Public or Accessibility.Internal } => true,
                _ => false
            },
            IsOfWrappedType = SymbolEqualityComparer.Default.Equals(valueProperty.Type, wrappedTypeSymbol)
        };
    }
    /// <summary>
    /// The property Name. 
    /// This will always be <c>Value</c>.
    /// </summary>
    public required string Name { get; init; }
    /// <summary>
    /// Indicates whether the property has a <see langword="public"/> or <see langword="internal"/> <see langword="set"/> or <see langword="init"/> setter.
    /// </summary>
    public required bool Initializable { get; init; }
    /// <summary>
    /// Indicates whether the property is of the wrapped type.
    /// </summary>
    public required bool IsOfWrappedType { get; init; }
}
