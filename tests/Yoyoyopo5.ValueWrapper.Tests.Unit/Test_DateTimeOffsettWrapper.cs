namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<DateTimeOffset>]
public readonly partial record struct DateTimeOffsetWrapper(DateTimeOffset Value) : ITestWrapper<DateTimeOffset, DateTimeOffsetWrapper>
{
    public static DateTimeOffset TestValue { get; } = DateTimeOffset.MinValue + TimeSpan.FromDays(23);
    public static DateTimeOffsetWrapper TestWrapper { get; } = new(DateTimeOffset.MinValue + TimeSpan.FromDays(20));
}

public class Test_DateTimeOffsetWrapperJsonConverter : Test_WrapperJsonConverter<DateTimeOffsetWrapper, DateTimeOffset>;
public class Test_DateTimeOffsetWrapperTypeConverter : Test_WrapperTypeConverter<DateTimeOffsetWrapper, DateTimeOffset>;
