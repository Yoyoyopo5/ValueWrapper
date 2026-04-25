namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<byte>]
public readonly partial record struct ByteWrapper(byte Value) : ITestWrapper<byte, ByteWrapper>
{
    public static byte TestValue { get; } = 2;
    public static ByteWrapper TestWrapper { get; } = new(128);
}

public class Test_ByteWrapperJsonConverter : Test_WrapperJsonConverter<ByteWrapper, byte>;
public class Test_ByteWrapperTypeConverter : Test_WrapperTypeConverter<ByteWrapper, byte>;
