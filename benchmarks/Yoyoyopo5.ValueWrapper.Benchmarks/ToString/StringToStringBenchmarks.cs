namespace Yoyoyopo5.ValueWrapper.Benchmarks.ToString;

public class StringToStringBenchmarks : ToStringBenchmarks<ColorName, string>
{
    protected override string Primitive { get; } = "red";
    protected override ColorName Wrapper { get; } = new("red");
}
