namespace Yoyoyopo5.ValueWrapper.Benchmarks.Json;

public class CoordinateJsonBenchmarks : JsonBenchmarks<PositiveCoordinate, Coordinate>
{
    protected override Coordinate Primitive { get; } = new(5, 5);
    protected override PositiveCoordinate Wrapper { get; } = new(new(5, 5));
}
