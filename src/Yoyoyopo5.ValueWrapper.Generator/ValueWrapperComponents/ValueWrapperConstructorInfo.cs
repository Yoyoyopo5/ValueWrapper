using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

/// <summary>
/// Contains information about a wrapper type's single wrapped parameter constructor.
/// </summary>
public readonly record struct ValueWrapperConstructorInfo
{
    /// <summary>
    /// Create a <see cref="ValueWrapperConstructorInfo"/> from a wrapper <see cref="INamedTypeSymbol"/>.
    /// </summary>
    /// <param name="symbol">The wrapper type.</param>
    /// <param name="wrappedTypeSymbol">The wrapped type.</param>
    /// <param name="ct" />
    /// <returns>
    /// A <see cref="ValueWrapperConstructorInfo"/> with info about the <paramref name="symbol"/>'s
    /// constructor taking a single parameter of the <paramref name="wrappedTypeSymbol"/> type,
    /// or <see langword="null"/> if such a constructor does not exist on the type.
    /// </returns>
    public static ValueWrapperConstructorInfo? FromWrapperType(
        INamedTypeSymbol symbol,
        INamedTypeSymbol wrappedTypeSymbol,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if (symbol.InstanceConstructors
            .Where(c => c.HasExactParametersOfType(wrappedTypeSymbol))
            .Select(static c => (IMethodSymbol?)c)
            .FirstOrDefault() is not IMethodSymbol constructor)
            return null;
        return new()
        {
            WrappedValueParameterName = constructor.Parameters.Single().Name,
            IsPrimaryConstructor = constructor.IsPrimaryConstructor(ct)
        };
    }
    /// <summary>
    /// The cosntructor's single parameter name.
    /// </summary>
    public required string WrappedValueParameterName { get; init; }
    /// <summary>
    /// Indicates whether the constructor is a primary constructor.
    /// </summary>
    public required bool IsPrimaryConstructor { get; init; }
}
