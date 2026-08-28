using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using Yoyoyopo5.ValueWrapper.Tests.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.Tests;

public class Test_ValueWrapperGenerator_Incremental
{
    private const string INCREMENTAL_TRACKING_NAME = ValueWrapperGenerator.TRACKING_NAME;

    private static readonly IncrementalTestCase[] _testCases =
    [
        new IncrementalTestCase()
        {
            Name = "Caches_WithAddedComment",
            Input = """
            namespace TestNamespace;
            using Yoyoyopo5.ValueWrapper;
            [Wrapper<int>]
            public partial record Caches_WithAddedComment;
            """,
            ModifiedInput = """
            namespace TestNamespace;
            using Yoyoyopo5.ValueWrapper;
            [Wrapper<int>]
            public partial record Caches_WithAddedComment; // Adds a comment.
            """,
            GeneratorTrackingName = INCREMENTAL_TRACKING_NAME,
            RunReason = IncrementalStepRunReason.Cached
        },
        new IncrementalTestCase()
        {
            Name = "Caches_WithAddedNonWrapperType",
            Input = """
            namespace TestNamespace;
            using Yoyoyopo5.ValueWrapper;
            [Wrapper<int>]
            public partial record Caches_WithAddedComment;
            """,
            ModifiedInput = """
            namespace TestNamespace;
            using Yoyoyopo5.ValueWrapper;
            [Wrapper<int>]
            public partial record Caches_WithAddedComment;

            public record OtherType(string Name);
            """,
            GeneratorTrackingName = INCREMENTAL_TRACKING_NAME,
            RunReason = IncrementalStepRunReason.Cached
        },
        new IncrementalTestCase()
        {
            Name = "DoesNotCache_WithAddedValueProperty",
            Input = """
            namespace TestNamespace;
            using Yoyoyopo5.ValueWrapper;
            [Wrapper<int>]
            public partial record Caches_WithAddedComment;
            """,
            ModifiedInput = """
            namespace TestNamespace;
            using Yoyoyopo5.ValueWrapper;
            [Wrapper<int>]
            public partial record Caches_WithAddedComment
            {
                public int Value { get; set; }
            }
            """,
            GeneratorTrackingName = INCREMENTAL_TRACKING_NAME,
            RunReason = IncrementalStepRunReason.Modified
        }
    ];
    public static TheoryData<TestCaseWrapper<IncrementalTestCase>> TestCases
        => [.. _testCases.Select(t => new TestCaseWrapper<IncrementalTestCase>(t))];

    private static async Task<CSharpCompilation> CompileAsync(SyntaxTree syntax, CancellationToken ct)
        => CSharpCompilation.Create(
            "Tests",
            [syntax],
            [.. await ReferenceAssemblies.Net.Net100.ResolveAsync(null, ct)]
            );

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task Generator(TestCaseWrapper<IncrementalTestCase> @case)
    {
        CancellationToken ct = TestContext.Current.CancellationToken;

        (SyntaxTree originalSyntax, SyntaxTree modifiedSyntax) = @case.TestCase.ParseSyntax();

        ct.ThrowIfCancellationRequested();
        (CSharpCompilation originalCompilation, CSharpCompilation modifiedCompilation)
            = (await CompileAsync(originalSyntax, ct), await CompileAsync(modifiedSyntax, ct));

        ct.ThrowIfCancellationRequested();
        CSharpGeneratorDriver driver = CSharpGeneratorDriver.Create(
            generators: [new ValueWrapperGenerator().AsSourceGenerator()],
            driverOptions: new GeneratorDriverOptions(
                IncrementalGeneratorOutputKind.None,
                trackIncrementalGeneratorSteps: true
                )
            );

        driver = (CSharpGeneratorDriver)driver.RunGenerators(originalCompilation, ct);
        GeneratorDriverRunResult runResult = driver.RunGenerators(modifiedCompilation, ct).GetRunResult();

        IncrementalStepRunReason[] runReasons = [.. runResult.Results.Single().TrackedSteps[INCREMENTAL_TRACKING_NAME]
            .SelectMany(s => s.Outputs)
            .Select(o => o.Reason)];

        Assert.Single(runReasons);
        Assert.Equal(@case.TestCase.RunReason, runReasons[0]);
    }
}
