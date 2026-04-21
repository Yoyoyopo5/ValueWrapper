namespace Yoyoyopo5.ValueWrapper.Benchmarks.Create;

public class DoubleCreateBenchmarks : CreateBenchmarks<Meters, double>
{
    protected override Meters CreateWrapper(double value) => new(value);
    protected override double CreatePrimitive() => 2.0;
}
