namespace Yoyoyopo5.ValueWrapper.Benchmarks.Json;

public class StringJsonBenchmarks : JsonBenchmarks<ColorName, string>
{
    protected override string Primitive { get; } = "green";
    protected override ColorName Wrapper { get; } = new("green");
}