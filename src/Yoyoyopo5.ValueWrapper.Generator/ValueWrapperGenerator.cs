using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System;
using System.Linq;
using System.IO;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;

namespace Yoyoyopo5.ValueWrapper.Generator;

public readonly record struct InjectedSource(string Filename, SourceText Source);

[Generator]
public class ValueWrapperGenerator : IIncrementalGenerator
{
    public const string TRACKING_NAME = "ValueWrapperDefinitionStep";

    private static string GetEmbeddedResource(string resourceName)
    {
        using Stream resourceStream 
            = typeof(ValueWrapperGenerator).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' was not found.");
        using StreamReader reader = new(resourceStream);
        return reader.ReadToEnd();
    }

    private static SourceText GetEmbeddedSource(string resourceName)
    {
        using Stream resourceStream 
            = typeof(ValueWrapperGenerator).Assembly.GetManifestResourceStream(resourceName)
            ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' was not found.");
        return SourceText.From(resourceStream, canBeEmbedded: true);
    }

    private const string INJECTED_SOURCE_PATH = "Yoyoyopo5.ValueWrapper.Core";
    public static ImmutableArray<InjectedSource> CoreSources = ImmutableArray.Create<InjectedSource>(
        new("IWrapValue.cs", GetEmbeddedSource($"{INJECTED_SOURCE_PATH}.IWrapValue.cs")),
        new("WrapperAttribute.cs", GetEmbeddedSource($"{INJECTED_SOURCE_PATH}.WrapperAttribute.cs")),
        new("WrapperJsonConverter.cs", GetEmbeddedSource($"{INJECTED_SOURCE_PATH}.WrapperJsonConverter.cs")),
        new("WrapperTypeConverter.cs", GetEmbeddedSource($"{INJECTED_SOURCE_PATH}.WrapperTypeConverter.cs"))
        );

    private readonly static Lazy<Template> _template 
        = new(() => Template.Parse(GetEmbeddedResource(ValueWrapperConstants.SCRIBAN_TEMPLATE_FILENAME)));
    private static Template ValueWrapperTemplate => _template.Value;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ValueWrapperDefinition?> provider = context.SyntaxProvider.ForAttributeWithMetadataName(
            ValueWrapperConstants.WRAPPER_ATTRIBUTE_NAME,
            predicate: static (node, _) => node is TypeDeclarationSyntax,
            transform: GeneratorAttributeSyntaxContextExtensions.ToValueWrapperDefinition
            )
            .Where(ValueWrapperDefinitionExtensions.ShouldRender)
            .WithTrackingName(TRACKING_NAME); // Enforces non-null wrapper definition

        context.RegisterPostInitializationOutput(static ctx =>
        {
            foreach (InjectedSource source in CoreSources)
            {
                ctx.AddSource(source);
            }
        });

        context.RegisterSourceOutput(provider, static (ctx, w) =>
        {
            ctx.CancellationToken.ThrowIfCancellationRequested();
            ctx.AddSource(w!.ToGeneratedSourceFilename("Wrapper.g.cs"), ValueWrapperTemplate.RenderPartialValueWrapper(w!));
        });
    }
}

public static class IncrementalGeneratorPostInitializationContextExtensions
{
    public static IncrementalGeneratorPostInitializationContext AddSource(
        this IncrementalGeneratorPostInitializationContext context,
        InjectedSource injectedSource
        )
    {
        context.AddSource(injectedSource.Filename, injectedSource.Source);
        return context;
    }
}