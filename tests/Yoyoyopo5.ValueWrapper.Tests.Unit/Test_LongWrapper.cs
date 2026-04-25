namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<long>]
public readonly partial record struct LongWrapper(long Value) : ITestWrapper<long, LongWrapper>
{
    public static long TestValue { get; } = -2;
    public static LongWrapper TestWrapper { get; } = new(500);
}

public class Test_LongWrapperJsonConverter : Test_WrapperJsonConverter<LongWrapper, long>;
public class Test_LongWrapperTypeConverter : Test_WrapperTypeConverter<LongWrapper, long>;