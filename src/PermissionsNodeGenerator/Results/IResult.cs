namespace PermissionsNodeGenerator.Results
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// The result of the operation.
        /// </summary>
        /// <remarks>The value depends on the implementation. It can be null, partial, or full.</remarks>
        object? Value { get; }

        /// <summary>
        /// Gets a value indicating if the operation succeeded.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Gets a value describing the result.
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// The generic interface for <see cref="IResult"/>.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public interface IResult<T> : IResult
    {
        /// <inheritdoc />
        new T? Value { get; }
    }
}
