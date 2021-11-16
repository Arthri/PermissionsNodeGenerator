using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This class exists for init-only setters to work.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit
    {
    }
}
