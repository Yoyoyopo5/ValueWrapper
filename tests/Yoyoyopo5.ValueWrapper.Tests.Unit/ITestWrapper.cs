namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

public interface ITestWrapper<out TWrapped, out TWrapper>
    where TWrapper : notnull
{
    static abstract TWrapped TestValue { get; }
    static abstract TWrapper TestWrapper { get; } 
    TWrapped Value { get; }
}
