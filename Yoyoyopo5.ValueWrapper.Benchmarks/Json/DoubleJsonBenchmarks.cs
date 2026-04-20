namespace Yoyoyopo5.ValueWrapper.Benchmarks.Json;

public class DoubleJsonBenchmarks : JsonBenchmarks<Meters, double>
{
    protected override double Primitive { get; } = 50.0;
    protected override Meters Wrapper { get; } = new(50.0);
}
