namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<string>]
public readonly partial record struct StringWrapper(string Value);

public class Test_StringWrapper : Test_WrapperJsonConverter<StringWrapper, string>
{
    protected override string? TestValue { get; } = "yoyo7tiger";
    protected override StringWrapper TestWrapper { get; } = new("yopobot lives");
}

public class Test_NullStringWrapper : Test_WrapperJsonConverter<StringWrapper, string>
{
    protected override string? TestValue { get; } = null;
    protected override StringWrapper TestWrapper { get; } = default;
}
