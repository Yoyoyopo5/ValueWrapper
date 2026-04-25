namespace Yoyoyopo5.ValueWrapper.Benchmarks.TypeConv;

public class StringTypeConverterBenchmarks : TypeConverterBenchmarks<ColorName, string>
{
    protected override string Primitive => "green";
    protected override ColorName Wrapper => new("green");
}
