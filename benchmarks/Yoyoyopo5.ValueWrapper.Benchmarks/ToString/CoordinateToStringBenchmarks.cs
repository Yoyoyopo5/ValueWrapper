namespace Yoyoyopo5.ValueWrapper.Benchmarks.ToString;

public class CoordinateToStringBenchmarks : ToStringBenchmarks<PositiveCoordinate, Coordinate>
{
    protected override Coordinate Primitive { get; } = new(2, 2);
    protected override PositiveCoordinate Wrapper { get; } = new(new(2, 2));
}
