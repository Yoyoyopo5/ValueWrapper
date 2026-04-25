namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<short>]
public readonly partial record struct ShortWrapper(short Value) : ITestWrapper<short, ShortWrapper>
{
    public static short TestValue => -12;
    public static ShortWrapper TestWrapper => new(64);
}

public class Test_ShortWrapperJsonConverter : Test_WrapperJsonConverter<ShortWrapper, short>;
public class Test_ShortWrapperTypeConverter : Test_WrapperTypeConverter<ShortWrapper, short>;
