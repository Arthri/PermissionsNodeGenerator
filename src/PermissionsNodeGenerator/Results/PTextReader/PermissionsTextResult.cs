using System.Collections.Generic;

namespace PermissionsNodeGenerator.Results.PTextReader
{
    /// <summary>
    /// Represents the result of a permissions text read operation.
    /// </summary>
    public record PermissionsTextResult : IResult<IList<PermissionNode>>
    {
        /// <inheritdoc />
        public IList<PermissionNode>? Value { get; init; }

        /// <inheritdoc />
        object? IResult.Value => Value;

        /// <inheritdoc />
        public bool Success { get; }

        /// <inheritdoc />
        public string Message { get; init; } = "";

        /// <summary>
        /// Initializes a new instance of <see cref="PermissionsTextResult"/>.
        /// </summary>
        /// <param name="success">The successfulness of the operation.</param>
        internal PermissionsTextResult(bool success)
        {
            Success = success;
        }
    }
}
