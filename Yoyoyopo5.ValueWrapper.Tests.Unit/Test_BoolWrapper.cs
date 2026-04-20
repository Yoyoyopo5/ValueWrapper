namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<bool>]
public readonly partial record struct BoolWrapper(bool Value);

public class Test_BoolWrapper : Test_WrapperJsonConverter<BoolWrapper, bool>
{
    protected override bool TestValue { get; } = false;
    protected override BoolWrapper TestWrapper { get; } = new(true);
}
