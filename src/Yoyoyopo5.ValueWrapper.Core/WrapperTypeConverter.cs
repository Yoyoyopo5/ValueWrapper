using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Yoyoyopo5.ValueWrapper;

/// <summary>
/// Type converter for wrapper types implementing <see cref="IWrapValue{T, TWrapper}"/>.
/// </summary>
/// <typeparam name="TWrapper">The wrapper type.</typeparam>
/// <typeparam name="TWrapped">The wrapped type.</typeparam>
internal class WrapperTypeConverter<TWrapper, TWrapped> : TypeConverter
    where TWrapper : IWrapValue<TWrapped, TWrapper>
{
    private static readonly TypeConverter _wrappedConverter = TypeDescriptor.GetConverter(typeof(TWrapped));

    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => sourceType == typeof(TWrapped)
        || _wrappedConverter.CanConvertFrom(context, sourceType)
        || base.CanConvertFrom(context, sourceType);

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        => destinationType == typeof(TWrapped)
        || _wrappedConverter.CanConvertTo(context, destinationType)
        || base.CanConvertTo(context, destinationType);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => value switch
        {
            TWrapped wrappedValue => wrappedValue,
            _ => (TWrapped?)_wrappedConverter.ConvertFrom(context, culture, value)
        } switch
        {
            TWrapped wrappedValue => TWrapper.Create(wrappedValue),
            _ => null
        };


    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => value switch
        {
            TWrapper wrapper when destinationType == typeof(TWrapped) => wrapper.Value,
            TWrapper wrapper => _wrappedConverter.ConvertTo(context, culture, wrapper.Value, destinationType),
            null => null,
            _ => base.ConvertTo(context, culture, value, destinationType)
        };
}
