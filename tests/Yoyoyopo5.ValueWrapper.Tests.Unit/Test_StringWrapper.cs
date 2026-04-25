namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<string>]
public readonly partial record struct StringWrapper(string Value) : ITestWrapper<string, StringWrapper>
{
    public static string TestValue => "yoyo7tiger";
    public static StringWrapper TestWrapper => new("yopobot lives");
}

public class Test_StringWrapperJsonConverter : Test_WrapperJsonConverter<StringWrapper, string>;
public class Test_NullStringWrapperJsonConverter : Test_WrapperJsonConverter<StringWrapper, string>
{
    protected override string TestValue { get; } = null!;
    protected override StringWrapper TestWrapper { get; } = default;
}
public class Test_StringWrapperTypeConverter : Test_WrapperTypeConverter<StringWrapper, string>;
