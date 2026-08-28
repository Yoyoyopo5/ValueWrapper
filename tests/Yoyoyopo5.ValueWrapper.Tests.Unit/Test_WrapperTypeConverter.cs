using System.ComponentModel;

namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

public abstract class IWrapValueProxy
{

}

public abstract class Test_WrapperTypeConverter<TWrapper, TWrapped>
    where TWrapper : ITestWrapper<TWrapped, TWrapper>
{
    protected virtual TWrapped TestValue { get; } = TWrapper.TestValue;
    protected virtual TWrapper TestWrapper { get; } = TWrapper.TestWrapper;
    protected virtual string? TestValueToString(TWrapped value) => value?.ToString();

    private readonly TypeConverter _converter = TypeDescriptor.GetConverter(typeof(TWrapper));

    [Fact]
    public void TypeConverter_RoundTripString_ReturnsWrapperWithSameValue()
    {
        if (_converter.ConvertToInvariantString(TestWrapper) is not string s)
        {
            Assert.Null(TestWrapper);
            return;
        }
        
        object? wrapper = _converter.ConvertFromInvariantString(s);
        Assert.IsType<TWrapper>(wrapper);
        Assert.Equal(TestWrapper.Value, ((TWrapper)wrapper).Value);
    }

    [Fact]
    public void TypeConverter_ConvertsFromWrappedType_ReturnsWrapper()
    {
        object? wrapper = TestValue switch
        {
            TWrapped value => _converter.ConvertFrom(value),
            _ => null
        };
        Assert.IsType<TWrapper>(wrapper);
        Assert.Equal(TestValue, ((TWrapper)wrapper).Value);
    }

    [Fact]
    public void TypeConverter_ConvertsFromString_ReturnsWrapper()
    {
        object? wrapper = TestValueToString(TestValue) switch
        {
            string stringValue => _converter.ConvertFrom(stringValue),
            _ => null
        };
        Assert.IsType<TWrapper>(wrapper);
        Assert.Equal(TestValue, ((TWrapper)wrapper).Value);
    }

    [Fact]
    public void TypeConverter_ConvertsToWrappedType_ReturnsValue()
    {
        object? value = _converter.ConvertTo(TestWrapper, typeof(TWrapped));
        Assert.IsType<TWrapped>(value);
        Assert.Equal(TestWrapper.Value, (TWrapped)value);
    }
}
