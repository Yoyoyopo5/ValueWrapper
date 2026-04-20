using Microsoft.CodeAnalysis;

namespace Yoyoyopo5.ValueWrapper.Roslyn.Shared;

public static class TypeKindExtensions
{
    public static string ToCodeString(this TypeKind typeKind)
        => typeKind switch
        {
            TypeKind.Struct or TypeKind.Structure => "struct",
            TypeKind.Class => "class",
            TypeKind.Interface => "interface",
            _ => string.Empty
        };
}