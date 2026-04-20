namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<ulong>]
public readonly partial record struct ULongWrapper(ulong Value);

public class Test_ULongWrapper : Test_WrapperJsonConverter<ULongWrapper, ulong>
{
    protected override ulong TestValue { get; } = 29371654;
    protected override ULongWrapper TestWrapper { get; } = new(500);
}
