namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<ulong>]
public readonly partial record struct ULongWrapper(ulong Value) : ITestWrapper<ulong, ULongWrapper>
{
    public static ulong TestValue => 1287;
    public static ULongWrapper TestWrapper => new(123787);
}

public class Test_ULongWrapperJsonConverter : Test_WrapperJsonConverter<ULongWrapper, ulong>;
public class Test_ULongWrapperTypeConverter : Test_WrapperTypeConverter<ULongWrapper, ulong>;
