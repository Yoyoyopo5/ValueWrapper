using System.Text.Json;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;
public abstract class Test_WrapperJsonConverter<TWrapper, TWrapped>
    where TWrapper : IWrapValue<TWrapped, TWrapper>, ITestWrapper<TWrapped, TWrapper>
{
    protected virtual TWrapped TestValue { get; } = TWrapper.TestValue;
    protected virtual TWrapper TestWrapper { get; } = TWrapper.TestWrapper;

    private readonly static JsonSerializerOptions options = new()
    {
        // We only need these for serializing/deserializing the raw values, not the wrapper type.
        Converters = { new JsonEncodedTextConverter(), new NintConverter(), new NUintConverter() }
    };

    [Fact]
    public void Read_WrappedValueType_ReturnsWrapperContainingValue()
    {
        string json = JsonSerializer.Serialize(TestValue, options);

        TWrapper? result = JsonSerializer.Deserialize<TWrapper>(json);

        Assert.Equal(TestValue, result is not null ? result.Value : default);
    }

    [Fact]
    public void Write_Wrapper_WritesWrappedValue()
    {
        string json = JsonSerializer.Serialize(TestWrapper is not null ? TestWrapper.Value : default, options);

        string result = JsonSerializer.Serialize(TestWrapper!);

        Assert.Equal(json, result);
    }

    [Fact]
    public void RoundTrip_Wrapper_PreservesValue()
    {
        string json = JsonSerializer.Serialize(TestWrapper);

        TWrapper? result = JsonSerializer.Deserialize<TWrapper>(json);

        Assert.Equal(TestWrapper is not null ? TestWrapper.Value : default, result is not null ? result.Value : default);
    }
}
