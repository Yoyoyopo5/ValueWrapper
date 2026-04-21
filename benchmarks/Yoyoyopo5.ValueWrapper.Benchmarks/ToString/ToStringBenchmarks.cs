using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Yoyoyopo5.ValueWrapper.Benchmarks.ToString;

[MemoryDiagnoser]
public abstract class ToStringBenchmarks<TWrapper, TWrapped>
    where TWrapper : IWrapValue<TWrapped, TWrapper>
{
    public static IReadOnlyList<Summary> Run()
    {
        List<Summary> summaries = [];
        summaries.Add(BenchmarkRunner.Run<CoordinateToStringBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<DoubleToStringBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<StringToStringBenchmarks>());
        return summaries;
    }
    protected abstract TWrapped Primitive { get; }
    protected abstract TWrapper Wrapper { get; }

    [Benchmark]
    public void WrapperToString() => Wrapper.ToString();

    [Benchmark(Baseline = true)]
    public string? PrimitiveToString() => Primitive?.ToString();
}
