namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<char>]
public readonly partial record struct CharWrapper(char Value) : ITestWrapper<char, CharWrapper>
{
    public static char TestValue { get; } = 'a';
    public static CharWrapper TestWrapper { get; } = new(' ');
}

public class Test_CharWrapperJsonConverter : Test_WrapperJsonConverter<CharWrapper, char>;
public class Test_CharWrapperTypeConverter : Test_WrapperTypeConverter<CharWrapper, char>;
