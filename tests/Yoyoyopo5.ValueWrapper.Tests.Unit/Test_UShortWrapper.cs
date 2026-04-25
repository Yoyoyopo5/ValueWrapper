namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<ushort>]
public readonly partial record struct UShortWrapper(ushort Value) 
    : ITestWrapper<ushort, UShortWrapper>
{
    public static UShortWrapper TestWrapper { get; } = new UShortWrapper(500);
    public static ushort TestValue { get; } = 127;
}

public sealed class Test_UShortWrapperJsonConverter : Test_WrapperJsonConverter<UShortWrapper, ushort>;
public sealed class Test_UShortWrapperTypeConverter : Test_WrapperTypeConverter<UShortWrapper, ushort>;
