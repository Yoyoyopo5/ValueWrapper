namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<float>]
public readonly partial record struct FloatWrapper(float Value);

public class Test_FloatWrapper : Test_WrapperJsonConverter<FloatWrapper, float>
{
    protected override float TestValue { get; } = 1.0f;
    protected override FloatWrapper TestWrapper { get; } = new(283.2f);
}
