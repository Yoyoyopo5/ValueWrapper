using Microsoft.CodeAnalysis;
using System.Threading;
using System.Linq;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct ToStringOverrideInfo
{
    public static ToStringOverrideInfo? FromTypeSymbol(INamedTypeSymbol symbol, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (symbol.GetMembers("ToString")
            .OfType<IMethodSymbol>()
            .Where(m => !m.IsImplicitlyDeclared)
            .FirstOrDefault(m => m.Parameters.Length == 0 && m.IsOverride) is null)
            return null;
        return new();
    }
}
