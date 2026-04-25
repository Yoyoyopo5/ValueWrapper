using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains information about the implicit out operator on a wrapper type.
/// </summary>
public readonly record struct ImplicitOperatorInfo
{
    /// <summary>
    /// Create an <see cref="ImplicitOperatorInfo"/> from a wrapper <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="symbol">The wrapper type.</param>
    /// <param name="wrappedTypeSymbol">The type that is being wrapped.</param>
    /// <param name="ct" />
    /// <returns>
    /// An <see cref="ImplicitOperatorInfo"/> about the <paramref name="symbol"/>'s implicit operator (wrapper => wrapped), 
    /// or <see langword="null"/> if no implicit operator was found.
    /// </returns>
    public static ImplicitOperatorInfo? FromTypeSymbol(
        INamedTypeSymbol symbol,
        ITypeSymbol wrappedTypeSymbol,
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
