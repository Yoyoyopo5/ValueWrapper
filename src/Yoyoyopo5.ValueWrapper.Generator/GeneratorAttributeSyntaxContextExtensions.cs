using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator;

internal static class GeneratorAttributeSyntaxContextExtensions
{
    public static ValueWrapperDefinition? ToValueWrapperDefinition(
        this GeneratorAttributeSyntaxContext context,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (context.TargetSymbol is not INamedTypeSymbol { IsStatic: false } wrapperTypeSymbol)
            return null;
        ct.ThrowIfCancellationRequested();
        if (context.Attributes.First().AttributeClass?.TypeArguments.FirstOrDefault() is not ITypeSymbol wrappedTypeSymbol)
            return null;
        ct.ThrowIfCancellationRequested();
        return wrapperTypeSymbol.ToValueWrapperDefinition(
            wrappedTypeSymbol,
            new AssemblySymbolContext()
            {
                JsonConverterAttribute = context.SemanticModel.Compilation.GetTypeByMetadataName(ValueWrapperConstants.JSON_CONVERTER_ATTRIBUTE_TYPE_NAME),
                TypeConverterAttribute = context.SemanticModel.Compilation.GetTypeByMetadataName(ValueWrapperConstants.TYPE_CONVERTER_ATTRIBUTE_TYPE_NAME)
            },
            ct
            );
    }
}
