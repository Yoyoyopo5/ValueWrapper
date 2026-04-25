namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<decimal>]
public readonly partial record struct DecimalWrapper(decimal Value) : ITestWrapper<decimal, DecimalWrapper>
{
    public static decimal TestValue { get; } = 123.2738m;
    public static DecimalWrapper TestWrapper { get; } = new(947.002783m);
}

public class Test_DecimalWrapperJsonConverter : Test_WrapperJsonConverter<DecimalWrapper, decimal>;
public class Test_DecimalWrapperTypeConverter : Test_WrapperTypeConverter<DecimalWrapper, decimal>;
