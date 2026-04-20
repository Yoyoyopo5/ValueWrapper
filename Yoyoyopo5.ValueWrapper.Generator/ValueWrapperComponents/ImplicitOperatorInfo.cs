using Microsoft.CodeAnalysis;
using System;
using System.Threading;
using System.Linq;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct ImplicitOperatorInfo
{
    public static ImplicitOperatorInfo? FromTypeSymbol(
        INamedTypeSymbol symbol,
        INamedTypeSymbol wrappedTypeSymbol,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (symbol.GetMembers()
            .OfType<IMethodSymbol>()
            .FirstOrDefault(m => m.MethodKind == MethodKind.Conversion
                && m.Name == "op_Implicit"
                && SymbolEqualityComparer.Default.Equals(m.ReturnType, wrappedTypeSymbol)) is null)
            return null;
        return new();
    }
}
