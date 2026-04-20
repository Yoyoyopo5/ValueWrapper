using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Analyzer.ValueWrapperComponents;

internal readonly record struct StaticCreateMethodInfo
{
    public static StaticCreateMethodInfo? FromWrapperSymbol(ValueWrapperSymbol wrapper)
    {
        if (wrapper.TypeSymbol
            .GetMembers("Create")
            .OfType<IMethodSymbol>()
            .Where(m => m.IsStatic)
            .Where(m => m.HasExactParametersOfType(wrapper.WrappedTypeSymbol))
            .FirstOrDefault(m => SymbolEqualityComparer.Default.Equals(m.ReturnType, wrapper.TypeSymbol)) is null)
            return null;
        return new();
    }
}
