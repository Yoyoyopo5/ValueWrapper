namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<DateTime>]
public readonly partial record struct DateTimeWrapper(DateTime Value);

public class Test_DateTimeWrapper : Test_WrapperJsonConverter<DateTimeWrapper, DateTime>
{
    protected override DateTime TestValue { get; } = DateTime.MinValue + TimeSpan.FromDays(2946);
    protected override DateTimeWrapper TestWrapper { get; } = new(DateTime.MaxValue);
}
