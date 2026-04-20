namespace Yoyoyopo5.ValueWrapper;

/// <summary>
/// Indicates the type is a wrapper around another type.
/// </summary>
/// <remarks>
/// This attribute informs the source generator to generate wrapper members for the type and add a JsonConverter.
/// Mark the type with <see langword="partial"/> to use.
/// </remarks>
/// <typeparam name="T">The type to wrap.</typeparam>
[AttributeUsage(validOn: AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class WrapperAttribute<T> : Attribute;
