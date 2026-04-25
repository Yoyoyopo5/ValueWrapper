namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<uint>]
public readonly partial record struct UIntWrapper(uint Value) : ITestWrapper<uint, UIntWrapper>
{
    public static uint TestValue => 127;
    public static UIntWrapper TestWrapper => new(124907);
}

public class Test_UIntWrapperJsonConverter : Test_WrapperJsonConverter<UIntWrapper, uint>;
public class Test_UIntWrapperTypeConverter : Test_WrapperTypeConverter<UIntWrapper, uint>;
