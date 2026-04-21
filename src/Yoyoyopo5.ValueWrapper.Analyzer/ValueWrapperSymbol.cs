using Microsoft.CodeAnalysis;
using Yoyoyopo5.ValueWrapper.Analyzer.ValueWrapperComponents;

namespace Yoyoyopo5.ValueWrapper.Analyzer;

internal readonly record struct ValueWrapperSymbol
{
    public required INamedTypeSymbol TypeSymbol { get; init; }
    public required INamedTypeSymbol WrappedTypeSymbol { get; init; }
}

internal static class ValueWrapperSymbolExtensions
{
    extension(ValueWrapperSymbol wrapper)
    {
        public StaticCreateMethodInfo? GetWrapperStaticCreateMethodOrDefault()
            => StaticCreateMethodInfo.FromWrapperSymbol(wrapper);

        public WrapperConstructorInfo? GetWrapperConstructorOrDefault()
            => WrapperConstructorInfo.FromWrapperSymbol(wrapper);

        public ValuePropertyInfo? GetWrapperValuePropertyOrDefault()
            => ValuePropertyInfo.FromWrapperSymbol(wrapper);
    }
}
