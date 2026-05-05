using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Yoyoyopo5.ValueWrapper.Benchmarks.Create;

[MemoryDiagnoser]
public abstract class CreateBenchmarks<TWrapper, TWrapped>
{
    public static IReadOnlyList<Summary> Run()
    {
        List<Summary> summaries = [];
        summaries.Add(BenchmarkRunner.Run<CoordinateCreateBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<DoubleCreateBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<StringCreateBenchmarks>());
        return summaries;
    }

    protected abstract TWrapper CreateWrapper(TWrapped value);
    protected abstract TWrapped CreatePrimitive();

    protected TWrapped _primitiveValue = default!;
    [GlobalSetup]
    public void Setup() => _primitiveValue = CreatePrimitive();

    [Benchmark]
    public TWrapper WrapperCreate() => CreateWrapper(_primitiveValue);

    [Benchmark(Baseline = true)]
    public TWrapped PrimitiveCreate() => _primitiveValue;
}
