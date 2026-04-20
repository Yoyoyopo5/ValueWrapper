namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<int>]
public readonly partial record struct IntWrapper(int Value);

public class Test_IntWrapper : Test_WrapperJsonConverter<IntWrapper, int>
{
    protected override int TestValue { get; } = -2;
    protected override IntWrapper TestWrapper { get; } = new(500);
}
