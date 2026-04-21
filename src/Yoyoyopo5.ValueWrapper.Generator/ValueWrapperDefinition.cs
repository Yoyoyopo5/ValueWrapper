using Microsoft.CodeAnalysis;
using System.Linq;
using Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator;

internal record ValueWrapperDefinition
{
    public required string Namespace { get; init; }
    public required bool IsGlobalNamespace { get; init; }
    public required string Name { get; init; }
    public required string PartialDeclaration { get; init; }
    public required bool IsPartial { get; init; }
    public required bool IsRecord { get; init; }
    public required bool HasJsonConverterAttribute { get; init; }
    public required bool HasOtherRequiredProperties { get; init; }
    public required bool HasEmptyConstructor { get; init; }
    public RecordImmutableArray<ParentTypeDefinition> ParentTypes { get; init; } = new();
    public required WrappedTypeInfo WrappedType { get; init; }
    public ValueWrapperConstructorInfo? WrapperConstructor { get; init; }
    public ValuePropertyInfo? WrapperValueProperty { get; init; }
    public ToStringOverrideInfo? ToStringOverride { get; init; }
    public ImplicitOperatorInfo? ImplicitOperator { get; init; }
    public StaticCreateMethodInfo? StaticCreateMethod { get; init; }
}

internal static class ValueWrapperDefinitionExtensions
{
    extension(ValueWrapperDefinition wrapper)
    {
        public bool ShouldAddValueProperty => wrapper switch
        {
            { IsRecord: true, WrapperConstructor: not null } or { WrapperValueProperty: not null } => false,
            _ => true
        };

        public bool ShouldRender => wrapper is not null
            && (wrapper.WrapperValueProperty is null or { IsOfWrappedType: true })
            && wrapper.IsPartial
            && wrapper.ParentTypes.All(p => p.IsPartial);

        public bool CanInitialize =>
            !wrapper.HasOtherRequiredProperties
            && wrapper.HasEmptyConstructor
            && (wrapper.WrapperValueProperty is not null and { Initializable: true } || wrapper.ShouldAddValueProperty);

        public string? JsonCreateExpression => wrapper switch // We actually supply the expression to create from value.
        {
            { StaticCreateMethod: not null } => "Create(value)",
            { WrapperConstructor: not null } => "new(value)",
            { CanInitialize: true } => $$"""new() { {{(wrapper.WrapperValueProperty.HasValue ? wrapper.WrapperValueProperty.Value.Name : "Value")}} = value }""",
            _ => null
        };
    }
}
