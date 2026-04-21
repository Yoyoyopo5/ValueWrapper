namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<Guid>]
public readonly partial record struct GuidWrapper(Guid Value);

public class Test_GuidWrapper : Test_WrapperJsonConverter<GuidWrapper, Guid>
{
    protected override Guid TestValue { get; } = Guid.Parse("317ad520-c87f-4577-b0aa-17245946ebe9");
    protected override GuidWrapper TestWrapper { get; } = new(Guid.Parse("d97682a1-9df5-4299-af84-229dda964983"));
}
