namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<DateTime>]
public readonly partial record struct DateTimeWrapper(DateTime Value) : ITestWrapper<DateTime, DateTimeWrapper>
{
    public static DateTime TestValue { get; } = DateTime.MinValue + TimeSpan.FromDays(2946);
    public static DateTimeWrapper TestWrapper { get; } = new(DateTime.MinValue + TimeSpan.FromDays(218));
}

public class Test_DateTimeWrapperJsonConverter : Test_WrapperJsonConverter<DateTimeWrapper, DateTime>;
public class Test_DateTimeWrapperTypeConverter : Test_WrapperTypeConverter<DateTimeWrapper, DateTime>;
