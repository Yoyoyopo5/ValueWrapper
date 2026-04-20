using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<JsonEncodedText>]
public readonly partial record struct JsonEncodedTextWrapper(JsonEncodedText Value);

public class Test_JsonEncodedTextWrapper : Test_WrapperJsonConverter<JsonEncodedTextWrapper, JsonEncodedText>
{
    protected override JsonEncodedText TestValue { get; } = JsonEncodedText.Encode("{ \"property\": \"value\" }");
    protected override JsonEncodedTextWrapper TestWrapper { get; } = new(JsonEncodedText.Encode("null"));
}

public class JsonEncodedTextConverter : JsonConverter<JsonEncodedText>
{
    public override JsonEncodedText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString() is string s ? JsonEncodedText.Encode(s) : default;

    public override void Write(Utf8JsonWriter writer, JsonEncodedText value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}