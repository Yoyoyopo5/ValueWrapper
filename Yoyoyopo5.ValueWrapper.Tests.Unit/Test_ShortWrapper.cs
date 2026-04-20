namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<short>]
public readonly partial record struct ShortWrapper(short Value);

public class Test_ShortWrapper : Test_WrapperJsonConverter<ShortWrapper, short>
{
    protected override short TestValue { get; } = -2;
    protected override ShortWrapper TestWrapper { get; } = new(500);
}
