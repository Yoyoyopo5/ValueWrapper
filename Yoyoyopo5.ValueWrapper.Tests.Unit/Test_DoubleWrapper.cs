namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<double>]
public readonly partial record struct DoubleWrapper(double Value);

public class Test_DoubleWrapper : Test_WrapperJsonConverter<DoubleWrapper, double>
{
    protected override double TestValue { get; } = 1.23;
    protected override DoubleWrapper TestWrapper { get; } = new(2.4782);
}
