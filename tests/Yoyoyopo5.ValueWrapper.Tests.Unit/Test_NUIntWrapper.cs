using System.Text.Json;
using System.Text.Json.Serialization;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<nuint>]
public readonly partial record struct NUIntWrapper(nuint Value) : ITestWrapper<nuint, NUIntWrapper>
{
    public static nuint TestValue { get; } = 287;
    public static NUIntWrapper TestWrapper { get; } = new(500);
}
public class NUintConverter : JsonConverter<nuint>
{
    public override nuint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => (nuint)reader.GetInt64();

    public override void Write(Utf8JsonWriter writer, nuint value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}

public class Test_NUIntWrapperJsonConverter : Test_WrapperJsonConverter<NUIntWrapper, nuint>;
// public class Test_NUIntWrapperTypeConverter : Test_WrapperTypeConverter<NUIntWrapper, nuint>;
// nuint TypeConverter does not support converting from string.
