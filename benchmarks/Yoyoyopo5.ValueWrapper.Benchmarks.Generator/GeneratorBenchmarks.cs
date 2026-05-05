using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Yoyoyopo5.ValueWrapper.Generator;

namespace Yoyoyopo5.ValueWrapper.Benchmarks.Generator;

[MemoryDiagnoser]
public class GeneratorBenchmarks
{
    private readonly GeneratorDriverOptions _driverOptions = new(
        disabledOutputs: IncrementalGeneratorOutputKind.None,
        trackIncrementalGeneratorSteps: true
        );

    private GeneratorDriver _generatorDriver = default!;
    private GeneratorDriver _ranGeneratorDriver = default!;
    private Compilation _compilation = default!;
    private Compilation _modifiedCompilation = default!;

    const string SOURCE_INPUT = """
        namespace Benchmarks;
        using Yoyoyopo5.ValueWrapper;

        public partial class Container
        {
            [Wrapper<int>]
            public readonly partial record struct BenchmarkWrapper;
        }
        """;

    const string MODIFIED_SOURCE_INPUT = """
        namespace Benchmarks;
        using Yoyoyopo5.ValueWrapper;

        public partial class Container // Added a comment.
        {
            [Wrapper<int>]
            public readonly partial record struct BenchmarkWrapper;
        }
        """;

    [GlobalSetup]
    public void Setup()
    {
        _compilation = CSharpCompilation.Create(
            "Benchmark",
            [CSharpSyntaxTree.ParseText(SOURCE_INPUT, path: "BenchmarkWrapper.cs")],
            [.. Basic.Reference.Assemblies.Net100.References.All.AsEnumerable<MetadataReference>()]
            );

        _modifiedCompilation = CSharpCompilation.Create(
            "Benchmark",
            [CSharpSyntaxTree.ParseText(MODIFIED_SOURCE_INPUT, path: "BenchmarkWrapper.cs")],
            [.. Basic.Reference.Assemblies.Net100.References.All.AsEnumerable<MetadataReference>()]
            );

        _generatorDriver = CSharpGeneratorDriver.Create(
            generators: [new ValueWrapperGenerator().AsSourceGenerator()],
            driverOptions: _driverOptions
            );

        _ranGeneratorDriver = _generatorDriver.RunGenerators(_compilation);

        if (GetDryRunError() is string error)
            throw new InvalidOperationException(error);

        EnsureCached();
    }

    public bool EnsureCached()
        => _ranGeneratorDriver.RunGenerators(_modifiedCompilation).GetRunResult()
            .Results
            .Single()
            .TrackedSteps[ValueWrapperGenerator.TRACKING_NAME]
            .SelectMany(s => s.Outputs)
            .Select(o => o.Reason)
            .Single() == IncrementalStepRunReason.Cached ? true : throw new InvalidOperationException("Second run was not cached!");

    public string? GetDryRunError()
        => _generatorDriver.RunGenerators(_compilation).GetRunResult() switch
        {
            { Diagnostics.IsEmpty: false } failed => $"Diagnostics were not empty: {string.Join(", ", failed.Diagnostics.Select(d => d.GetMessage()))}",
            { GeneratedTrees.IsEmpty: true } => "No source code was generated.",
            _ => null
        };

    [Benchmark]
    public void RunGenerator()
        => _generatorDriver.RunGenerators(_compilation);

    [Benchmark]
    public void RunGeneratorCached()
        => _ranGeneratorDriver.RunGenerators(_modifiedCompilation);
}
