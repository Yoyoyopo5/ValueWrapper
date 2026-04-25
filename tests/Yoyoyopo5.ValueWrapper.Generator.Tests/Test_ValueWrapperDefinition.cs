using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Yoyoyopo5.ValueWrapper.Generator.ValueWrapperComponents;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.Tests;

public class Test_ValueWrapperDefinition
{
    private const string WRAPPER_NAME = "TestWrapper";

    private static readonly ValueWrapperDefinition TestWrapper = new()
    {
        Namespace = "TestNamespace",
        StaticCreateMethod = new(),
        ToStringOverride = new(),
        HasEmptyConstructor = true,
        HasJsonConverterAttribute = true,
        HasTypeConverterAttribute = false,
        HasOtherRequiredProperties = false,
        ImplicitOperator = null,
        IsGlobalNamespace = false,
        IsPartial = true,
        IsRecord = true,
        Name = WRAPPER_NAME,
        ParentTypes = ImmutableArray.Create(new ParentTypeDefinition() { Name = "TestParent", IsPartial = true, PartialDeclaration = "public partial record TestParent" }).ToRecordImmutableArray(),
        PartialDeclaration = "public partial record TestWrapper",
        WrappedType = new() { FullyQualifiedName = "global::System.String", IsNullable = false, IsValueType = true, TypeKind = TypeKind.Class },
        WrapperConstructor = new() { IsPrimaryConstructor = true, WrappedValueParameterName = "Value" },
        WrapperValueProperty = new() { Initializable = true, IsOfWrappedType = true, Name = "Value" }
    };

    [Fact]
    public void Definition_WithSameData_AreEqual()
    {
        ValueWrapperDefinition b = TestWrapper with { Name = WRAPPER_NAME };

        Assert.Equal(TestWrapper, b);
    }

    [Fact]
    public void Definition_WithDifferentData_AreNotEqual()
    {
        ValueWrapperDefinition b = TestWrapper with { Name = "Other" };

        Assert.NotEqual(TestWrapper, b);
    }
}
