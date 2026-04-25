using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<nint>]
public readonly partial record struct NIntWrapper(nint Value) : ITestWrapper<nint, NIntWrapper>
{
    public static nint TestValue { get; } = -2;
    public static NIntWrapper TestWrapper { get; } = new(500);
}
public class NintConverter : JsonConverter<nint>
{
    public override nint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => (nint)reader.GetInt64();

    public override void Write(Utf8JsonWriter writer, nint value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}

public class Test_NIntWrapperJsonConverter : Test_WrapperJsonConverter<NIntWrapper, nint>;
// public class Test_NIntWrapperTypeConverter : Test_WrapperTypeConverter<NIntWrapper, nint>;
// nint TypeConverter does not support converting from string.

