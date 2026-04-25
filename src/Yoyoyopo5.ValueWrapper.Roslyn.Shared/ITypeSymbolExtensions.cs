using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Yoyoyopo5.ValueWrapper.Roslyn.Shared;

public static class ITypeSymbolExtensions
{
    public static bool IsNullable(this ITypeSymbol symbol)
        => symbol switch
        {
            { IsValueType: true } vt => vt.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T,
            _ => symbol.NullableAnnotation == NullableAnnotation.Annotated
        };
}