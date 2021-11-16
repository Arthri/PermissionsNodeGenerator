using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace PermissionsNodeGenerator.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="GeneratorExecutionContext"/>.
    /// </summary>
    public static class GeneratorExecutionContextExtensions
    {
        /// <summary>
        /// Reports the specified diagnostics to the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="diagnostics">The diagnostics.</param>
        /// <remarks>There is no guarantee whoever is receiving this diagnostics will sort them in the correct order.</remarks>
        public static void ReportDiagnostic(this GeneratorExecutionContext context, IEnumerable<Diagnostic> diagnostics)
        {
            foreach (var diagnostic in diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
