using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PermissionsNodeGenerator.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Diagnostic"/>.
    /// </summary>
    public static class DiagnosticExtensions
    {
        /// <summary>
        /// Splits the specified diagnostic into multiple diagnostics.
        /// </summary>
        /// <param name="diagnostic">The diagnostic to split.</param>
        public static IEnumerable<Diagnostic> SplitMultilineDiagnostic(this Diagnostic diagnostic)
        {
            var lines = diagnostic
                .GetMessage()
                .Split(
                    new[]
                    {
                        Environment.NewLine
                    },
                    StringSplitOptions.RemoveEmptyEntries);

            string[] customTags = diagnostic.Descriptor.CustomTags.ToArray();
            foreach (var line in lines)
            {
                var partialDiagnostic = Diagnostic.Create(
                    new DiagnosticDescriptor(
                        diagnostic.Id,
                        diagnostic.Descriptor.Title,
                        line,
                        diagnostic.Descriptor.Category,
                        diagnostic.Severity,
                        diagnostic.Descriptor.IsEnabledByDefault,
                        diagnostic.Descriptor.Description,
                        diagnostic.Descriptor.HelpLinkUri,
                        customTags),
                    diagnostic.Location);

                yield return partialDiagnostic;
            }
        }

        /// <summary>
        /// Reports the diagnostic to the specified context.
        /// </summary>
        /// <param name="diagnostic">The diagnostic.</param>
        /// <param name="context">The context.</param>
        /// <returns>The diagnostic reported.</returns>
        public static Diagnostic ReportTo(this Diagnostic diagnostic, GeneratorExecutionContext context)
        {
            context.ReportDiagnostic(diagnostic);
            return diagnostic;
        }

        /// <summary>
        /// Reports the diagnostics to the specified context.
        /// </summary>
        /// <param name="diagnostics">The diagnostics.</param>
        /// <param name="context">The context.</param>
        /// <returns>The diagnostics reported.</returns>
        public static IEnumerable<Diagnostic> ReportTo(this IEnumerable<Diagnostic> diagnostics, GeneratorExecutionContext context)
        {
            context.ReportDiagnostic(diagnostics);
            return diagnostics;
        }
    }
}
