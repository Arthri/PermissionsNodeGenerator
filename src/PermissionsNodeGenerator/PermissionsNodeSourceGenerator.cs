using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Represents a source code generator for Permission Nodes.
    /// </summary>
    [Generator]
    public class PermissionsNodeSourceGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        /// <inheritdoc />
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                var targetFiles = context
                    .AdditionalFiles
                    .Where(f =>
                        context
                            .AnalyzerConfigOptions
                            .GetOptions(f)
                            .TryGetValue("build_metadata.AdditionalFiles.SourceItemGroup", out var sourceItemGroup)
                        && sourceItemGroup == "PermissionsText");

                foreach (var file in targetFiles)
                {
                    IReadOnlyList<PermissionNode> nodes;


                    var sourceText = file.GetText(context.CancellationToken);
                    var text = sourceText.ToString();

                    using (var stream = new MemoryStream())
                    {
                        using (var reader = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                        {
                            reader.Write(text);
                        }

                        stream.Seek(0, SeekOrigin.Begin);

                        try
                        {
                            nodes = PermissionTextReader.Parse(stream);
                        }
                        catch (Exception e)
                        {
                            ReportCantParseExceptionDiagnostic(context, e, file.Path);
                            continue;
                        }
                    }


                    var className = Path.GetFileNameWithoutExtension(file.Path);
                    var generatedSource = PermissionsClassTemplater.GenerateTemplate(nodes, className);

                    context.AddSource(
                        $"{className}.g.cs",
                        generatedSource);
                }
            }
            catch (Exception e)
            {
                ReportUnhandledExceptionDiagnostic(context, e);
            }
        }

        private static void ReportMultilineDiagnostic(
            GeneratorExecutionContext context,
            Diagnostic diagnostic)
        {
            var lines = diagnostic
                .GetMessage()
                .Split(
                    new[]
                    {
                        Environment.NewLine
                    },
                    StringSplitOptions.RemoveEmptyEntries);

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
                        diagnostic.Descriptor.CustomTags.ToArray()),
                    diagnostic.Location);

                context.ReportDiagnostic(partialDiagnostic);
            }
        }

        private static void ReportUnhandledExceptionDiagnostic(GeneratorExecutionContext context, Exception e)
        {
            var diagnostic = Diagnostic.Create(
                new DiagnosticDescriptor(
                    "PNSG0001",
                    "Unhandled exception thrown while generating source.",
                    e.ToString(),
                    "Other",
                    DiagnosticSeverity.Error,
                    true),
                Location.None);

            ReportMultilineDiagnostic(context, diagnostic);
        }

        private static void ReportCantParseExceptionDiagnostic(GeneratorExecutionContext context, Exception e, string path)
        {
            var diagnostic = Diagnostic.Create(
                new DiagnosticDescriptor(
                    "PNSG1001",
                    "Unhandled exception thrown while reading permissions text.",
                    e.ToString(),
                    "Format",
                    DiagnosticSeverity.Error,
                    true),
                Location.Create(
                    path,
                    new TextSpan(),
                    new LinePositionSpan()));

            ReportMultilineDiagnostic(context, diagnostic);
        }
    }
}
