using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit.Sdk;
using Yoyoyopo5.ValueWrapper.Tests.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator.Tests;

public record IncrementalTestCase : ITestData<IncrementalTestCase>
{
    public required string Name { get; init; }
    public required string Input { get; init; }
    public required string ModifiedInput { get; init; }
    public required string GeneratorTrackingName { get; init; }
    public required IncrementalStepRunReason RunReason { get; init; }

    public static IncrementalTestCase Deserialize(IXunitSerializationInfo info)
        => new()
        {
            Name = info.GetValue<string>(nameof(Name)) ?? throw new ArgumentNullException(nameof(info), "Name cannot be null"),
            Input = info.GetValue<string>(nameof(Input)) ?? throw new ArgumentNullException(nameof(info), "Input cannot be null"),
            ModifiedInput = info.GetValue<string>(nameof(ModifiedInput)) ?? throw new ArgumentNullException(nameof(info), "ModifiedInput cannot be null"),
            GeneratorTrackingName = info.GetValue<string>(nameof(GeneratorTrackingName)) ?? throw new ArgumentNullException(nameof(info), "GeneratorTrackingName cannot be null"),
            RunReason = info.GetValue<IncrementalStepRunReason>(nameof(RunReason))
        };

    public IXunitSerializationInfo Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(Name), Name);
        info.AddValue(nameof(Input), Input);
        info.AddValue(nameof(ModifiedInput), ModifiedInput);
        info.AddValue(nameof(GeneratorTrackingName), GeneratorTrackingName);
        info.AddValue(nameof(RunReason), RunReason);
        return info;
    }

    public override string ToString() => Name;
}

public static class IncrementalTestCaseExtensions
{
    public static (SyntaxTree original, SyntaxTree modified) ParseSyntax(this IncrementalTestCase @case)
        => (CSharpSyntaxTree.ParseText(@case.Input), CSharpSyntaxTree.ParseText(@case.ModifiedInput));
}
