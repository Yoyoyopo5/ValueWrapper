namespace Yoyoyopo5.ValueWrapper.Benchmarks;

public readonly record struct Coordinate(int X, int Y);

[Wrapper<Coordinate>]
public readonly partial record struct PositiveCoordinate(Coordinate Value);
