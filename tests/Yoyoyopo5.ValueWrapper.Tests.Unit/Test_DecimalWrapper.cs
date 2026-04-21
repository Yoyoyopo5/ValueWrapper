namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<decimal>]
public readonly partial record struct DecimalWrapper(decimal Value);

public class Test_DecimalWrapper : Test_WrapperJsonConverter<DecimalWrapper, decimal>
{
    protected override decimal TestValue { get; } = 123.2738m;
    protected override DecimalWrapper TestWrapper { get; } = new(947.002783m);
}
