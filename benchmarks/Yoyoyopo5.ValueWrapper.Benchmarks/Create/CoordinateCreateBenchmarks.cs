namespace Yoyoyopo5.ValueWrapper.Benchmarks.Create;

public class CoordinateCreateBenchmarks : CreateBenchmarks<PositiveCoordinate, Coordinate>
{
    protected override PositiveCoordinate CreateWrapper(Coordinate value) => new(value);
    protected override Coordinate CreatePrimitive() => new(2, 2);
}