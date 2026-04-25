using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<JsonEncodedText>]
public readonly partial record struct JsonEncodedTextWrapper(JsonEncodedText Value) : ITestWrapper<JsonEncodedText, JsonEncodedTextWrapper>
{
    public static JsonEncodedText TestValue { get; } = JsonEncodedText.Encode("{ \"property\": \"value\" }");
    public static JsonEncodedTextWrapper TestWrapper { get; } = new(JsonEncodedText.Encode("null"));
}
public class JsonEncodedTextConverter : JsonConverter<JsonEncodedText>
{
    public override JsonEncodedText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() is string s ? JsonEncodedText.Encode(s) : default;

    public override void Write(Utf8JsonWriter writer, JsonEncodedText value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}

public class Test_JsonEncodedTextWrapperJsonConverter : Test_WrapperJsonConverter<JsonEncodedTextWrapper, JsonEncodedText>;
// public class Test_JsonEncodedTextWrapperTypeConverter : Test_WrapperTypeConverter<JsonEncodedTextWrapper, JsonEncodedText>;
// JsonEncodedText TypeConverter does not support converting from string.
