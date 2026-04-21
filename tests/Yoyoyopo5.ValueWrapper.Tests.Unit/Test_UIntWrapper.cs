namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<uint>]
public readonly partial record struct UIntWrapper(uint Value);

public class Test_UIntWrapper : Test_WrapperJsonConverter<UIntWrapper, uint>
{
    protected override uint TestValue { get; } = 1000;
    protected override UIntWrapper TestWrapper { get; } = new(500);
}
