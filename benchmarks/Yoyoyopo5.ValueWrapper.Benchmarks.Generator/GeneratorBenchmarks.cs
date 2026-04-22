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
    private GeneratorDriver _secondRunGeneratorDriver = default!;
    private GeneratorDriver _vanillaDriver = default!;
    private Compilation _compilation = default!;
    private Compilation _afterFirstRunCompilation = default!;
    private Compilation _incrementallyModifiedCompilation = default!;
    private Compilation _addedWrapperCompilation = default!;

    const string SOURCE_INPUT = """
        namespace Benchmarks;
        using Yoyoyopo5.ValueWrapper;

        public partial class Container
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
            [
                ..Basic.Reference.Assemblies.Net100.References.All.AsEnumerable<MetadataReference>(),
                MetadataReference.CreateFromFile(typeof(WrapperAttribute<>).Assembly.Location)
                ]
            );

        _generatorDriver = CSharpGeneratorDriver.Create(
            generators: [new ValueWrapperGenerator().AsSourceGenerator()],
            driverOptions: _driverOptions
            );

        _secondRunGeneratorDriver = _generatorDriver.RunGeneratorsAndUpdateCompilation(_compilation, out _afterFirstRunCompilation, out _);

        _incrementallyModifiedCompilation = _afterFirstRunCompilation.AddSyntaxTrees(
            CSharpSyntaxTree.ParseText("// Nothing here", path: "Comment.cs")
            );

        _addedWrapperCompilation = _afterFirstRunCompilation.AddSyntaxTrees(
            CSharpSyntaxTree.ParseText("""
                namespace Benchmarks;
                using Yoyoyopo5.ValueWrapper;
                [Wrapper<int>]
                public readonly partial record struct BenchmarkWrapper2;
                """,
                path: "BenchmarkWrapper2.cs")
            );

        _vanillaDriver = CSharpGeneratorDriver.Create(
            generators: [],
            driverOptions: _driverOptions
            );

        if (GetDryRunError() is string error)
            throw new InvalidOperationException(error);
    }

    public string? GetDryRunError()
        => _generatorDriver.RunGenerators(_compilation).GetRunResult() switch
        {
            { Diagnostics.IsEmpty: false } failed => $"Diagnostics were not empty: {string.Join(", ", failed.Diagnostics.Select(d => d.GetMessage()))}",
            { GeneratedTrees.IsEmpty: true } => "No source code was generated.",
            _ => null
        };

    [Benchmark(Baseline = true)]
    public void NoGenerator()
        => _vanillaDriver.RunGenerators(_compilation);

    [Benchmark]
    public void RunGenerator()
        => _generatorDriver.RunGenerators(_compilation);

    [Benchmark]
    public void RunGeneratorAgain()
        => _secondRunGeneratorDriver.RunGenerators(_addedWrapperCompilation);

    [Benchmark]
    public void RunGeneratorIncrementalChange()
        => _secondRunGeneratorDriver.RunGenerators(_incrementallyModifiedCompilation);
}
