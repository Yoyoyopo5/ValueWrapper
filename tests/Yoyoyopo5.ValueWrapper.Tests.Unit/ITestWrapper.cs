namespace Yoyoyopo5.ValueWrapper.Tests.Unit;

public interface ITestWrapper<out TWrapped, out TWrapper>
    where TWrapper : notnull, IWrapValue<TWrapped, TWrapper>
{
    static abstract TWrapped TestValue { get; }
    static abstract TWrapper TestWrapper { get; } 
}
