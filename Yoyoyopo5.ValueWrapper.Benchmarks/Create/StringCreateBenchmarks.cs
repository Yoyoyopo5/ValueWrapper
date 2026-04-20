namespace Yoyoyopo5.ValueWrapper.Benchmarks.Create;

public class StringCreateBenchmarks : CreateBenchmarks<ColorName, string>
{
    protected override ColorName CreateWrapper(string value) => new(value);
    protected override string CreatePrimitive() => "primitive";
}
