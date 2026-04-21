using Microsoft.CodeAnalysis;
using System.Threading;
using System.Linq;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains information about a wrapper type's <c>ToString()</c> override method.
/// </summary>
public readonly record struct ToStringOverrideInfo
{
    /// <summary>
    /// Create a <see cref="ToStringOverrideInfo"/> from a wrapper <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <remarks>
    /// This does not include implicitly declared <c>ToString()</c> methods, 
    /// such as those added by the <see langword="record"/> syntax.
    /// </remarks>
    /// <param name="symbol">The wrapper type.</param>
    /// <param name="ct" />
    /// <returns>
    /// A <see cref="ToStringOverrideInfo"/> with info about the <paramref name="symbol"/>'s overriden <c>ToString()</c>,
    /// or <see langword="null"/> if it does not exist on the <paramref name="symbol"/>.
    /// </returns>
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
