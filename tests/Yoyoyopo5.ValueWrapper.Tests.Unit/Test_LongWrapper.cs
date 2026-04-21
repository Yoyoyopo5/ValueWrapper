namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<long>]
public readonly partial record struct LongWrapper(long Value);

public class Test_LongWrapper : Test_WrapperJsonConverter<LongWrapper, long>
{
    protected override long TestValue { get; } = -2;
    protected override LongWrapper TestWrapper { get; } = new(500);
}
