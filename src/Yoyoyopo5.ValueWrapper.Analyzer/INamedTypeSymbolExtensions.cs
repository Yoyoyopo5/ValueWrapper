using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Yoyoyopo5.ValueWrapper.Analyzer;

internal static partial class INamedTypeSymbolExtensions
{
    private static AttributeData? GetAttributesOfType(this INamedTypeSymbol symbol, INamedTypeSymbol attributeTypeSymbol)
        => symbol.GetAttributes().FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass?.OriginalDefinition, attributeTypeSymbol.OriginalDefinition));

    public static ValueWrapperSymbol? AsValueWrapperSymbol(this INamedTypeSymbol symbol, INamedTypeSymbol wrapperAttributeTypeSymbol)
        => symbol.GetAttributesOfType(wrapperAttributeTypeSymbol) switch
        {
            AttributeData wrapperAttributeData => wrapperAttributeData.AttributeClass?.TypeArguments.FirstOrDefault() switch
            {
                INamedTypeSymbol wrappedTypeSymbol => new ValueWrapperSymbol()
                {
                    TypeSymbol = symbol,
                    WrappedTypeSymbol = wrappedTypeSymbol,
                },
                _ => null
            },
            _ => null
        };
}
