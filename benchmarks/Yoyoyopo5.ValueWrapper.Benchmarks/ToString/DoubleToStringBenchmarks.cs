namespace Yoyoyopo5.ValueWrapper.Benchmarks.ToString;

public class DoubleToStringBenchmarks : ToStringBenchmarks<Meters, double>
{
    protected override double Primitive { get; } = 2.0;
    protected override Meters Wrapper { get; } = new(2.0);
}
