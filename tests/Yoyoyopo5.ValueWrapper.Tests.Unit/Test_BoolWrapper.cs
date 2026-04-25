namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<bool>]
public readonly partial record struct BoolWrapper(bool Value) : ITestWrapper<bool, BoolWrapper>
{
    public static bool TestValue { get; } = false;
    public static BoolWrapper TestWrapper { get; } = new(true);
}

public class Test_BoolWrapperJsonConverter : Test_WrapperJsonConverter<BoolWrapper, bool>;
public class Test_BoolWrapperTypeConverter : Test_WrapperTypeConverter<BoolWrapper, bool>;
