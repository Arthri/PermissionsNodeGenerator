using Microsoft.CodeAnalysis.Diagnostics;

namespace PermissionsNodeGenerator.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="AnalyzerConfigOptionsProvider"/>.
    /// </summary>
    public static class AnalyzerConfigOptionsProviderExtensions
    {
        /// <summary>
        /// Returns the value of an MSBuild property with the specified name.
        /// </summary>
        /// <param name="analyzerConfigOptions">The analyzer options.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The value of the specified MSBuild property.</returns>
        public static string GetProperty(this AnalyzerConfigOptionsProvider analyzerConfigOptions, string propertyName)
        {
            analyzerConfigOptions
                .GlobalOptions
                .TryGetValue(
                    $"build_property.{propertyName}",
                    out var result);

            return result;
        }
    }
}
