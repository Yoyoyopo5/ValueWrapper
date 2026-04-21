using Scriban;
using System;
using System.Linq;
using Yoyoyopo5.ValueWrapper.Roslyn.Shared;

namespace Yoyoyopo5.ValueWrapper.Generator;

internal static class TemplateExtensions
{
    public static string RenderPartialValueWrapper(
        this Template wrapperTemplate,
        ValueWrapperDefinition wrapper)
        => wrapperTemplate.Render(
            new
            {
                Namespace = wrapper.IsGlobalNamespace ? null : wrapper.Namespace,
                TypeName = wrapper.Name,
                Parents = wrapper.ParentTypes.Reverse().Select(pt => pt.PartialDeclaration).ToArray(),
                WrappedTypeName = wrapper.WrappedType.FullyQualifiedName,
                PartialDefinition = wrapper.PartialDeclaration,
                JsonConverterType = wrapper.HasJsonConverterAttribute ? null : ValueWrapperConstants.WRAPPER_JSON_CONVERTER_NAME.Replace("`2", $"<{wrapper.Name}, {wrapper.WrappedType.FullyQualifiedName}>"),
                wrapper.ShouldAddValueProperty,
                ShouldAddImplicitOperator = wrapper.ImplicitOperator is null,
                ShouldAddToStringOverride = wrapper.ToStringOverride is null,
                WrapperInterface = ValueWrapperConstants.WRAPPER_INTERFACE_NAME.Replace("`2", $"<{wrapper.WrappedType.FullyQualifiedName}, {wrapper.Name}>"),
                wrapper.JsonCreateExpression,
                WrappedTypeIsNullable = wrapper.WrappedType.IsNullable
            });
}
