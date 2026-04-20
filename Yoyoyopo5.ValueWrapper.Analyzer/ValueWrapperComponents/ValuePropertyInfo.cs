using Microsoft.CodeAnalysis;
using System.Linq;

namespace Yoyoyopo5.ValueWrapper.Analyzer.ValueWrapperComponents;

internal readonly record struct ValuePropertyInfo
{
    public static ValuePropertyInfo? FromWrapperSymbol(ValueWrapperSymbol wrapper)
    {
        if (wrapper.TypeSymbol.GetMembers("Value")
            .OfType<IPropertySymbol>()
            .FirstOrDefault() is not IPropertySymbol valueProperty)
            return null;
        return new()
        {
            Initializable = valueProperty.SetMethod switch
            {
                { DeclaredAccessibility: Accessibility.Public or Accessibility.Internal } => true,
                _ => false
            },
            IsOfWrappedType = SymbolEqualityComparer.Default.Equals(valueProperty.Type, wrapper.WrappedTypeSymbol)
        };
    }
    public required bool Initializable { get; init; }
    public required bool IsOfWrappedType { get; init; }
}
