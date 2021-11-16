using Microsoft.CodeAnalysis.Diagnostics;

namespace PermissionsNodeGenerator.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="AnalyzerConfigOptions"/>.
    /// </summary>
    public static class AnalyzerConfigOptionsExtensions
    {
        /// <summary>
        /// Returns the value of an MSBuild item metadata with the specified name.
        /// </summary>
        /// <param name="analyzerConfigOptions">The analyzer options.</param>
        /// <param name="metadataName">The metadata name.</param>
        /// <returns>The value of the specified MSBuild item metadata.</returns>
        public static string? GetMetadata(this AnalyzerConfigOptions analyzerConfigOptions, string metadataName)
        {
            analyzerConfigOptions
                .TryGetValue(
                    $"build_metadata.AdditionalFiles.{metadataName}",
                    out var result);

            return result;
        }
    }
}
