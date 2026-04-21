using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<nint>]
public readonly partial record struct NIntWrapper(nint Value);

public class Test_NIntWrapper : Test_WrapperJsonConverter<NIntWrapper, nint>
{
    protected override nint TestValue { get; } = -2;
    protected override NIntWrapper TestWrapper { get; } = new(500);
}

public class NintConverter : JsonConverter<nint>
{
    public override nint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => (nint)reader.GetInt64();

    public override void Write(Utf8JsonWriter writer, nint value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}
