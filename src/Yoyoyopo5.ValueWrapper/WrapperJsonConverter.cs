using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper;

/// <summary>
/// Used to serialize basic wrapper classes and structs that implement <see cref="IWrapValue{T, TWrapper}"/>.
/// </summary>
public class WrapperJsonConverter<TWrapper, TWrapped> : JsonConverter<TWrapper>
    where TWrapper : IWrapValue<TWrapped, TWrapper>
{
    private static TWrapped AsWrapped<T>(ref T value)
        => Unsafe.As<T, TWrapped>(ref value);
    private static T AsWrapped<T>(ref TWrapped value)
        => Unsafe.As<TWrapped, T>(ref value);
    /// <inheritdoc/>
    public override TWrapper? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return default;

        if (typeof(TWrapped) == typeof(string))
        {
            string? value = reader.GetString();
            return value is null ? default : TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(int))
        {
            int value = reader.GetInt32();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(uint))
        {
            uint value = reader.GetUInt32();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(byte))
        {
            byte value = reader.GetByte();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(sbyte))
        {
            sbyte value = reader.GetSByte();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(long))
        {
            long value = reader.GetInt64();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(ulong))
        {
            ulong value = reader.GetUInt64();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(short))
        {
            short value = reader.GetInt16();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(ushort))
        {
            ushort value = reader.GetUInt16();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(nint))
        {
            nint value = (nint)reader.GetInt64();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(nuint))
        {
            nuint value = (nuint)reader.GetUInt64();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(decimal))
        {
            decimal value = reader.GetDecimal();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(double))
        {
            double value = reader.GetDouble();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(float))
        {
            float value = reader.GetSingle();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(bool))
        {
            bool value = reader.GetBoolean();
            return TWrapper.Create(AsWrapped(ref value));
        }
        if (typeof(TWrapped) == typeof(char))
        {
            string? s = reader.GetString();
            if (string.IsNullOrEmpty(s))
                return default;
            char c = s[0];
            return TWrapper.Create(AsWrapped(ref c));
        }

        else if (typeof(TWrapped) == typeof(DateTime))
        {
            DateTime value = reader.GetDateTime();
            return TWrapper.Create(AsWrapped(ref value));
        }
        else if (typeof(TWrapped) == typeof(DateTimeOffset))
        {
            DateTimeOffset value = reader.GetDateTimeOffset();
            return TWrapper.Create(AsWrapped(ref value));
        }
        else if (typeof(TWrapped) == typeof(Guid))
        {
            Guid value = reader.GetGuid();
            return TWrapper.Create(AsWrapped(ref value));
        }
        else if (typeof(TWrapped) == typeof(JsonEncodedText))
        {
            if (reader.GetString() is not string s)
                return default;
            JsonEncodedText value = JsonEncodedText.Encode(s);
            return TWrapper.Create(AsWrapped(ref value));
        }

        return JsonSerializer.Deserialize<TWrapped>(ref reader, options) switch
        {
            { } value => TWrapper.Create(value),
            _ => default,
        };
    }
    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TWrapper value, JsonSerializerOptions options)
    {
        TWrapped wrappedValue = value.Value;

        if (typeof(TWrapped) == typeof(string))
            writer.WriteStringValue(AsWrapped<string>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(int))
            writer.WriteNumberValue(AsWrapped<int>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(uint))
            writer.WriteNumberValue(AsWrapped<uint>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(byte))
            writer.WriteNumberValue(AsWrapped<byte>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(sbyte))
            writer.WriteNumberValue(AsWrapped<sbyte>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(long))
            writer.WriteNumberValue(AsWrapped<long>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(ulong))
            writer.WriteNumberValue(AsWrapped<ulong>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(short))
            writer.WriteNumberValue(AsWrapped<short>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(ushort))
            writer.WriteNumberValue(AsWrapped<ushort>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(nint))
            writer.WriteNumberValue(AsWrapped<nint>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(nuint))
            writer.WriteNumberValue(AsWrapped<nuint>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(decimal))
            writer.WriteNumberValue(AsWrapped<decimal>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(double))
            writer.WriteNumberValue(AsWrapped<double>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(float))
            writer.WriteNumberValue(AsWrapped<float>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(bool))
            writer.WriteBooleanValue(AsWrapped<bool>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(char))
            writer.WriteStringValue([AsWrapped<char>(ref wrappedValue)]);

        else if (typeof(TWrapped) == typeof(DateTime))
            writer.WriteStringValue(AsWrapped<DateTime>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(DateTimeOffset))
            writer.WriteStringValue(AsWrapped<DateTimeOffset>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(Guid))
            writer.WriteStringValue(AsWrapped<Guid>(ref wrappedValue));
        else if (typeof(TWrapped) == typeof(JsonEncodedText))
            writer.WriteStringValue(AsWrapped<JsonEncodedText>(ref wrappedValue));

        else // catches other types and nullables
            JsonSerializer.Serialize(writer, wrappedValue, options);
    }
}
