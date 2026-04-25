namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<Guid>]
public readonly partial record struct GuidWrapper(Guid Value) : ITestWrapper<Guid, GuidWrapper>
{
    public static Guid TestValue { get; } = Guid.Parse("317ad520-c87f-4577-b0aa-17245946ebe9");
    public static GuidWrapper TestWrapper { get; } = new(Guid.Parse("d97682a1-9df5-4299-af84-229dda964983"));
}

public class Test_GuidWrapperJsonConverter : Test_WrapperJsonConverter<GuidWrapper, Guid>;
public class Test_GuidWrapperTypeConverter : Test_WrapperTypeConverter<GuidWrapper, Guid>;
