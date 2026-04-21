using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct StaticCreateMethodInfo
{
    public static StaticCreateMethodInfo? FromTypeSymbol(
        INamedTypeSymbol symbol,
        INamedTypeSymbol wrappedTypeSymbol,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (symbol
            .GetMembers("Create")
            .OfType<IMethodSymbol>()
            .Where(m => m.IsStatic)
            .Where(m => m.HasExactParametersOfType(wrappedTypeSymbol))
            .FirstOrDefault(m => SymbolEqualityComparer.Default.Equals(m.ReturnType, symbol)) is null)
            return null;
        return new();
    }
}
