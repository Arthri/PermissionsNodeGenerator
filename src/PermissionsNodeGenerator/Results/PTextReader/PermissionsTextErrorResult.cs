using System;

namespace PermissionsNodeGenerator.Results.PTextReader
{
    /// <summary>
    /// Represents the result of a failed permissions text read.
    /// </summary>
    public record PermissionsTextErrorResult : PermissionsTextResult
    {
        /// <summary>
        /// Gets the line where the error occured.
        /// </summary>
        public int Line { get; init; }

        /// <summary>
        /// Gets the position where the error occured.
        /// </summary>
        public int LinePositionStart { get; init; }

        /// <summary>
        /// Gets the position where the error occured.
        /// </summary>
        public int LinePositionEnd { get; init; }

        /// <summary>
        /// Gets the exception thrown.
        /// </summary>
        public Exception? Exception { get; init; }

        /// <summary>
        /// Initializes a new instance of <see cref="PermissionsTextErrorResult"/>.
        /// </summary>
        internal PermissionsTextErrorResult() : base(false)
        {
        }
    }
}
