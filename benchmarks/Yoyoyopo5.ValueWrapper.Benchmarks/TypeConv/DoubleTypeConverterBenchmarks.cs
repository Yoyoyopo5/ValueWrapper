namespace Yoyoyopo5.ValueWrapper.Benchmarks.TypeConv;

public class DoubleTypeConverterBenchmarks : TypeConverterBenchmarks<Meters, double>
{
    protected override double Primitive => 3.0;
    protected override Meters Wrapper => new(3.0);
}