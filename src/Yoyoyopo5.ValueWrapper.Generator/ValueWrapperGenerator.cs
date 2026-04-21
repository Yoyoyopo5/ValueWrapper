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
            );

        context.RegisterSourceOutput(provider, (ctx, w) =>
        {
            if (w is null || !w.ShouldRender)
                return;
            ctx.CancellationToken.ThrowIfCancellationRequested();
            ctx.AddSource($"{(w!.IsGlobalNamespace ? string.Empty : w!.Namespace)}_{string.Join("_", w!.ParentTypes.Reverse().Select(p => p.Name))}{(w.ParentTypes.Any() ? "_" : "")}{w!.Name}_Wrapper.g.cs", ValueWrapperTemplate.RenderPartialValueWrapper(w));
        });
    }
}
