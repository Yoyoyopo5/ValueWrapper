namespace Yoyoyopo5.ValueWrapper;

/// <summary>
/// Implemented automatically by source generator when using <see cref="WrapperAttribute{T}"/>.
/// </summary>
/// <remarks>
/// This interface enables the use of the <see cref="WrapperJsonConverter{TWrapper, TWrapped}"/>.
/// </remarks>
public interface IWrapValue<T, out TWrapper>
{
    /// <summary>
    /// The wrapped value.
    /// </summary>
    T Value { get; }
    /// <summary>
    /// Create an instance of the wrapper using the wrapped value.
    /// </summary>
    /// <param name="value">The value that should be wrapped.</param>
    /// <returns>A new instance of <typeparamref name="TWrapper"/> wrapping <paramref name="value"/>.</returns>
    static abstract TWrapper Create(T value);
}
