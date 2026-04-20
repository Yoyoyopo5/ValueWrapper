using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct ValuePropertyInfo
{
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
    public required string Name { get; init; }
    public required bool Initializable { get; init; }
    public required bool IsOfWrappedType { get; init; }
}
