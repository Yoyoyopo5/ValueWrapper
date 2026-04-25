namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<double>]
public readonly partial record struct DoubleWrapper(double Value) : ITestWrapper<double, DoubleWrapper>
{
    public static double TestValue { get; } = 1.23;
    public static DoubleWrapper TestWrapper { get; } = new(2.4782);
}

public class Test_DoubleWrapperJsonConverter : Test_WrapperJsonConverter<DoubleWrapper, double>;
public class Test_DoubleWrapperTypeConverter : Test_WrapperTypeConverter<DoubleWrapper, double>;
