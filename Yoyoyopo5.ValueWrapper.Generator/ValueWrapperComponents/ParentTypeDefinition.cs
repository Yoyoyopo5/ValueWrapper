using Microsoft.CodeAnalysis;
using System.Threading;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;

public readonly record struct ParentTypeDefinition
{
    public static ParentTypeDefinition FromTypeSymbol(INamedTypeSymbol parentTypeSymbol, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return new()
        {
            Name = parentTypeSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
            PartialDeclaration = parentTypeSymbol.ToPartialTypeDeclarationString(),
            IsPartial = parentTypeSymbol.IsPartial()
        };
    }
    public required string Name { get; init; }
    public required string PartialDeclaration { get; init; }
    public required bool IsPartial { get; init; }
}
