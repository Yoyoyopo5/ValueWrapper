namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

public record TestRecord(string Name, int Amount);

[Wrapper<TestRecord>]
public partial class TestRecordWrapper;

public class Test_RecordWrapper : Test_WrapperJsonConverter<TestRecordWrapper, TestRecord>
{
    protected override TestRecord? TestValue { get; } = new("wallace", 23);
    protected override TestRecordWrapper? TestWrapper { get; } = new() { Value = new("walter", -2) };
}

public class Test_NullRecordWrapper : Test_WrapperJsonConverter<TestRecordWrapper, TestRecord>
{
    protected override TestRecord? TestValue { get; } = null;
    protected override TestRecordWrapper? TestWrapper { get; } = null;
}
