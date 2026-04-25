using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains information about the <see langword="static"/> <c>Wrapper Create(WrappedType value)</c> method on a wrapper type.
/// </summary>
public readonly record struct StaticCreateMethodInfo
{
    /// <summary>
    /// Get <see cref="StaticCreateMethodInfo"/> from a wrapper <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="symbol">The wrapper type.</param>
    /// <param name="wrappedTypeSymbol">The wrapped type.</param>
    /// <param name="ct" />
    /// <returns>
    /// A <see cref="StaticCreateMethodInfo"/> with info about the <paramref name="symbol"/> <c>Create</c> method,
    /// or <see langword="null"/> if the method does not exist on the wrapper type.
    /// </returns>
    public static StaticCreateMethodInfo? FromTypeSymbol(
        INamedTypeSymbol symbol,
        ITypeSymbol wrappedTypeSymbol,
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
