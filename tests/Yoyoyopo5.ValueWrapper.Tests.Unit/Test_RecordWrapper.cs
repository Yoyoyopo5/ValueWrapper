using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[TypeConverter(typeof(TestRecordTypeConverter))]
public record TestRecord(string Name, int Amount);

public class TestRecordTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string) 
        || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            string stringValue => JsonSerializer.Deserialize<TestRecord>(stringValue),
            _ => base.ConvertFrom(context, culture, value)
        };

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        => destinationType == typeof(string)
        || base.CanConvertTo(context, destinationType);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        TestRecord? tr = (TestRecord?)value;
        if (destinationType == typeof(string))
            return JsonSerializer.Serialize(tr);
        return base.ConvertTo(context, culture, value, destinationType);
    }
}

[Wrapper<TestRecord>]
public partial class TestRecordWrapper : ITestWrapper<TestRecord, TestRecordWrapper>
{
    public static TestRecord TestValue => new("wallace", 23);
    public static TestRecordWrapper TestWrapper => new() { Value = new("walter", -2) };
}

public class Test_RecordWrapperJsonConverter : Test_WrapperJsonConverter<TestRecordWrapper, TestRecord>;
public class Test_NullRecordWrapperJsonConverter : Test_WrapperJsonConverter<TestRecordWrapper, TestRecord>
{
    protected override TestRecord TestValue { get; } = null!;
    protected override TestRecordWrapper TestWrapper { get; } = null!;
}
public class Test_RecordWrapperTypeConverter : Test_WrapperTypeConverter<TestRecordWrapper, TestRecord>
{
    protected override string? TestValueToString(TestRecord value)
        => JsonSerializer.Serialize(value);
}
