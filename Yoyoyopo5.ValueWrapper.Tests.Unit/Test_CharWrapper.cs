namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

[Wrapper<char>]
public readonly partial record struct CharWrapper(char Value);

public class Test_CharWrapper : Test_WrapperJsonConverter<CharWrapper, char>
{
    protected override char TestValue { get; } = 'a';
    protected override CharWrapper TestWrapper { get; } = new(' ');
}
