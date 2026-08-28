using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using System.ComponentModel;

namespace Yoyoyopo5.ValueWrapper.Benchmarks.TypeConv;

[MemoryDiagnoser]
public abstract class TypeConverterBenchmarks<TWrapper, TWrapped>
{
    public static IReadOnlyList<Summary> Run()
    {
        List<Summary> summaries = [];
        summaries.Add(BenchmarkRunner.Run<CoordinateTypeConverterBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<DoubleTypeConverterBenchmarks>());
        summaries.Add(BenchmarkRunner.Run<StringTypeConverterBenchmarks>());
        return summaries;
    }

    protected abstract TWrapped Primitive { get; }
    protected abstract TWrapper Wrapper { get; }

    private static readonly TypeConverter WrappedConverter = TypeDescriptor.GetConverter(typeof(TWrapped));
    private static readonly TypeConverter WrapperConverter = TypeDescriptor.GetConverter(typeof(TWrapper));

    [Benchmark]
    public void WrapperRoundTripString()
    {
        string s = WrapperConverter.ConvertToInvariantString(Wrapper)!;
        WrapperConverter.ConvertFromInvariantString(s);
    }

    [Benchmark(Baseline = true)]
    public void PrimitiveRoundTripString()
    {
        string s = WrappedConverter.ConvertToInvariantString(Primitive)!;
        WrapperConverter.ConvertFromInvariantString(s);
    }
}
