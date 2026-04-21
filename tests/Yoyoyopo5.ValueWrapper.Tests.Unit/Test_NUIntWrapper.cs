using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<nuint>]
public readonly partial record struct NUIntWrapper(nuint Value);

public class Test_NUIntWrapper : Test_WrapperJsonConverter<NUIntWrapper, nuint>
{
    protected override nuint TestValue { get; } = 287;
    protected override NUIntWrapper TestWrapper { get; } = new(500);
}

public class NUintConverter : JsonConverter<nuint>
{
    public override nuint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => (nuint)reader.GetInt64();

    public override void Write(Utf8JsonWriter writer, nuint value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}
