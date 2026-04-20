namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<byte>]
public readonly partial record struct ByteWrapper(byte Value);

public class Test_ByteWrapper : Test_WrapperJsonConverter<ByteWrapper, byte>
{
    protected override byte TestValue { get; } = 2;
    protected override ByteWrapper TestWrapper { get; } = new(128);
}
