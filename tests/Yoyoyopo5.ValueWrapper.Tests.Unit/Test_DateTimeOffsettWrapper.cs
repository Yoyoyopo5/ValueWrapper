namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<DateTimeOffset>]
public readonly partial record struct DateTimeOffsetWrapper(DateTimeOffset Value);

public class Test_DateTimeOffsetWrapper : Test_WrapperJsonConverter<DateTimeOffsetWrapper, DateTimeOffset>
{
    protected override DateTimeOffset TestValue { get; } = DateTimeOffset.MinValue + TimeSpan.FromDays(23);
    protected override DateTimeOffsetWrapper TestWrapper { get; } = new(DateTimeOffset.MaxValue);
}
