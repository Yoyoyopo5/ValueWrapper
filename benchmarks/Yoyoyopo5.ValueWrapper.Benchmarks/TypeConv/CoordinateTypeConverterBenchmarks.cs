namespace Yoyoyopo5.ValueWrapper.Benchmarks.TypeConv;

public class CoordinateTypeConverterBenchmarks : TypeConverterBenchmarks<PositiveCoordinate, Coordinate>
{
    protected override Coordinate Primitive => new(2,3);
    protected override PositiveCoordinate Wrapper => new(new(2,3));
}
