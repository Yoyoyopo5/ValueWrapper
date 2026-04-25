namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<sbyte>]
public readonly partial record struct SByteWrapper(sbyte Value) : ITestWrapper<sbyte, SByteWrapper>
{
    public static sbyte TestValue => 16;
    public static SByteWrapper TestWrapper => new(-32);
}

public class Test_SByteWrapperJsonConverter : Test_WrapperJsonConverter<SByteWrapper, sbyte>;
public class Test_SByteWrapperTypeConverter : Test_WrapperTypeConverter<SByteWrapper, sbyte>;