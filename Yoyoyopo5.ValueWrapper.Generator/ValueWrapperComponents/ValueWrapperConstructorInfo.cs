using Microsoft.CodeAnalysis;
using System.Linq;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct ValueWrapperConstructorInfo
{
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
    public required string WrappedValueParameterName { get; init; }
    public required bool IsPrimaryConstructor { get; init; }
}
