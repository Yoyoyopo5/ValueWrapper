namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<ushort>]
public readonly partial record struct UShortWrapper(ushort Value);

public class Test_UShortWrapper : Test_WrapperJsonConverter<UShortWrapper, ushort>
{
    protected override ushort TestValue { get; } = 127;
    protected override UShortWrapper TestWrapper { get; } = new(500);
}
