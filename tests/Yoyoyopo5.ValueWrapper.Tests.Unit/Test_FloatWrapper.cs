namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<float>]
public readonly partial record struct FloatWrapper(float Value) : ITestWrapper<float, FloatWrapper>
{
    public static float TestValue { get; } = 1.0f;
    public static FloatWrapper TestWrapper { get; } = new(283.2f);
}

public class Test_FloatWrapperJsonConverter : Test_WrapperJsonConverter<FloatWrapper, float>;
public class Test_FloatWrapperTypeConverter : Test_WrapperTypeConverter<FloatWrapper, float>;
