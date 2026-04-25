namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<int>]
public readonly partial record struct IntWrapper(int Value) : ITestWrapper<int, IntWrapper>
{
    public static int TestValue { get; } = -2;
    public static IntWrapper TestWrapper { get; } = new(500);
}

public class Test_IntWrapperJsonConverter : Test_WrapperJsonConverter<IntWrapper, int>;
public class Test_IntWrapperTypeConverter : Test_WrapperTypeConverter<IntWrapper, int>;
