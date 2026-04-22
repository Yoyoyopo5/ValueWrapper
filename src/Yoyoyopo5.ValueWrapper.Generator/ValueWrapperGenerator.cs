using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Scriban;
using System;
using System.Linq;
using System.IO;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator;

[Generator]
public class ValueWrapperGenerator : IIncrementalGenerator
{
    public const string TRACKING_NAME = "ValueWrapperDefinitionStep";

    private readonly static Lazy<Template> _template = new(() =>
    {
        if (typeof(ValueWrapperGenerator).Assembly.GetManifestResourceStream(ValueWrapperConstants.SCRIBAN_TEMPLATE_FILENAME) is not Stream templateStream)
            throw new FileNotFoundException($"Scriban template file for {nameof(ValueWrapperGenerator)} was not found.");
        string templateString;
        using (StreamReader reader = new(templateStream))
        {
            templateString = reader.ReadToEnd();
        }
        return Template.Parse(templateString);
    });
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

        context.RegisterSourceOutput(provider, static (ctx, w) =>
        {
            ctx.CancellationToken.ThrowIfCancellationRequested();
            ctx.AddSource(w!.ToGeneratedSourceFilename("Wrapper.g.cs"), ValueWrapperTemplate.RenderPartialValueWrapper(w!));
            //ctx.AddSource(w!.ToGeneratedSourceFilename("Wrapper.g.cs"), "// Hello");
        });
    }
}
