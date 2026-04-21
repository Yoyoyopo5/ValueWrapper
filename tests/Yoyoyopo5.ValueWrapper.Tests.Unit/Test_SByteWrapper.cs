namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<sbyte>]
public readonly partial record struct SByteWrapper(sbyte Value);

public class Test_SByteWrapper : Test_WrapperJsonConverter<SByteWrapper, sbyte>
{
    protected override sbyte TestValue { get; } = -2;
    protected override SByteWrapper TestWrapper { get; } = new(8);
}
