using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.Text.Json;

namespace Yoyoyopo5.ValueWrapper.Benchmarks.Json;

[MemoryDiagnoser]
public abstract class JsonBenchmarks<TWrapper, TWrapped>
    where TWrapper : IWrapValue<TWrapped, TWrapper>
{
    public static IReadOnlyList<Summary> Run()
    {
        List<Summary> summaries = [];
        summaries.Add(BenchmarkRunner.Run<CoordinateJsonBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<DoubleJsonBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<StringJsonBenchmarks>());
        return summaries;
    }
    protected abstract TWrapped Primitive { get; }
    protected abstract TWrapper Wrapper { get; }

    [Benchmark]
    public void WrapperRoundTripJson()
    {
        string json = JsonSerializer.Serialize(Wrapper);
        JsonSerializer.Deserialize<TWrapper>(json);
    }

    [Benchmark(Baseline = true)]
    public void PrimitiveRoundTripJson()
    {
        string json = JsonSerializer.Serialize(Primitive);
        JsonSerializer.Deserialize<TWrapped>(json);
    }
}
