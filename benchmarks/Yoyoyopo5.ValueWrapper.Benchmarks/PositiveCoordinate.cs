using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;

namespace Yoyoyopo5.ValueWrapper.Benchmarks;

[TypeConverter(typeof(CoordinateTypeConverter))]
public readonly record struct Coordinate(int X, int Y);

public class CoordinateTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(string)
        || base.CanConvertFrom(context, sourceType);
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            string stringValue => JsonSerializer.Deserialize<Coordinate>(stringValue),
            _ => base.ConvertFrom(context, culture, value)
        };

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        => destinationType == typeof(string)
        || base.CanConvertTo(context, destinationType);
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => value switch
        {
            Coordinate coordinate when destinationType == typeof(string) => JsonSerializer.Serialize(coordinate),
            _ => base.ConvertTo(context, culture, value, destinationType)
        };
}

[Wrapper<Coordinate>]
public readonly partial record struct PositiveCoordinate(Coordinate Value);
