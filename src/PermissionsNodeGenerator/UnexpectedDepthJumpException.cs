using System;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// The exception thrown when the depth shifts unexpectedly.
    /// </summary>
    public class UnexpectedDepthJumpException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UnexpectedDepthJumpException"/>.
        /// </summary>
        public UnexpectedDepthJumpException() : base("Unexpected depth jump")
        {
        }
    }
}
