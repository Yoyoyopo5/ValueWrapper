using Microsoft.CodeAnalysis;
using System.Linq;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Analyzer.ValueWrapperComponents;

internal readonly record struct WrapperConstructorInfo
{
    public static WrapperConstructorInfo? FromWrapperSymbol(ValueWrapperSymbol wrapper)
    {
        if (wrapper.TypeSymbol.InstanceConstructors
            .Where(c => c.HasExactParametersOfType(wrapper.WrappedTypeSymbol))
            .Select(static c => (IMethodSymbol?)c)
            .FirstOrDefault() is not IMethodSymbol constructor)
            return null;
        return new();
    }
}
