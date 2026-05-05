using BenchmarkDotNet.Attributes;
using System.Text.Json;

namespace Yoyoyopo5.ValueWrapper.Benchmarks.Json;

[MemoryDiagnoser]
public class DictionaryKeyJsonBenchmarks
{
    private readonly Dictionary<ColorName, string> _wrapperDict = new()
    {
        [new ColorName("blue")] = "value"
    };

    private readonly Dictionary<string, string> _stringDict = new()
    {
        ["blue"] = "value"
    };

    [Benchmark(Baseline = true)]
    public void StringKeyDictionaryRoundTripJson()
    {
        string json = JsonSerializer.Serialize(_stringDict);
        JsonSerializer.Deserialize<Dictionary<string, string>>(json);
    }

    [Benchmark]
    public void WrapperKeyDictionaryRoundTripJson()
    {
        string json = JsonSerializer.Serialize(_wrapperDict);
        JsonSerializer.Deserialize<Dictionary<ColorName, string>>(json);
    }
}
