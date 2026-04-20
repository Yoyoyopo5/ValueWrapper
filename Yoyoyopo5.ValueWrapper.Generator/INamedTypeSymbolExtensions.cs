using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator;

internal static class INamedTypeSymbolExtensions
{
    public static ValueWrapperDefinition? ToValueWrapperDefinition(
        this INamedTypeSymbol typeSymbol,
        INamedTypeSymbol wrappedTypeSymbol,
        INamedTypeSymbol? jsonConverterAttributeSymbol,
        CancellationToken ct)
        => new()
        {
            Namespace = typeSymbol.ContainingNamespace.ToDisplayString(),
            Name = typeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            IsPartial = typeSymbol.IsPartial(),
            IsRecord = typeSymbol.IsRecord,
            PartialDeclaration = typeSymbol.ToPartialTypeDeclarationString(),
            WrappedType = WrappedTypeInfo.FromTypeSymbol(wrappedTypeSymbol, ct),
            ParentTypes = typeSymbol.GetContainingTypes().Select(t => ParentTypeDefinition.FromTypeSymbol(t, ct)).ToRecordImmutableArray(),
            WrapperConstructor = ValueWrapperConstructorInfo.FromWrapperType(typeSymbol, wrappedTypeSymbol, ct),
            WrapperValueProperty = ValuePropertyInfo.FromTypeSymbol(typeSymbol, wrappedTypeSymbol, ct),
            ImplicitOperator = ImplicitOperatorInfo.FromTypeSymbol(typeSymbol, wrappedTypeSymbol, ct),
            ToStringOverride = ToStringOverrideInfo.FromTypeSymbol(typeSymbol, ct),
            HasJsonConverterAttribute = typeSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, jsonConverterAttributeSymbol)),
            StaticCreateMethod = StaticCreateMethodInfo.FromTypeSymbol(typeSymbol, wrappedTypeSymbol, ct),
            HasEmptyConstructor = typeSymbol.InstanceConstructors.Any(c => c.Parameters.IsEmpty),
            HasOtherRequiredProperties = typeSymbol.GetMembers().OfType<IPropertySymbol>().Where(p => p.IsRequired).Any(p => p.Name != "Value")
        };
}
