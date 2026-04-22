using Microsoft.CodeAnalysis;

namespace Yoyoyopo5.ValueWrapper.Analyzer;

internal static class ValueWrapperDiagnostics
{
    public static readonly DiagnosticDescriptor PartialModifierRequiredWarning = new(
        id: "VWG0001",
        title: "Wrapper type must be partial",
        messageFormat: "Make '{0}' partial to enable wrapper generation",
        category: "ValueWrapper",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public static readonly DiagnosticDescriptor PartialParentTypesRequiredWarning = new(
        id: "VWG0002",
        title: "Wrapper containing types must be partial",
        messageFormat: "Make '{0}' partial to enable wrapper generation for '{1}'",
        category: "ValueWrapper",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public static readonly DiagnosticDescriptor JsonConstructionMethodMissingWarning = new(
        id: "VWG0003",
        title: "Wrapper type is not constructable",
        messageFormat: "To enable wrapper Json deserialization, '{0}' must have: " +
        "(1) a public constructor taking a single argument of the wrapped type, " +
        "(2) a publicly initializable Value property of the wrapped type, or " +
        "(3) a static Create method taking a single argument of the wrapped type and returning an instance of {0}",
        category: "ValueWrapper",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public static readonly DiagnosticDescriptor WronglyTypedValueProperty = new(
        id: "VWG0004",
        title: "Value property is not wrapped type",
        messageFormat: "'{0}' must have a Value property or record primary constructor of type '{1}' to enable wrapper generation",
        category: "ValueWrapper",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );

    public static readonly DiagnosticDescriptor StaticWrapperTypeWarning = new(
        id: "VWG0005",
        title: "Wrapper types cannot be static",
        messageFormat: "'{0}' must be non-static to enable wrapper generation",
        category: "ValueWrapper",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
        );
}
