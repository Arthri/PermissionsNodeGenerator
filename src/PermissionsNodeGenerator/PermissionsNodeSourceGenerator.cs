using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using PermissionsNodeGenerator.Extensions;
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
                for (int i = 0; i < context.AdditionalFiles.Length; i++)
                {
                    var file = context.AdditionalFiles[i];
                    var fileOptions = context
                        .AnalyzerConfigOptions
                        .GetOptions(file);
                    var fileSourceItemGroup = fileOptions.GetMetadata("SourceItemGroup");

                    if (fileSourceItemGroup != "PermissionsText")
                    {
                        continue;
                    }

                    var settings = CreateSettings(fileOptions);

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
                            nodes = PermissionTextReader.Parse(stream, settings);
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

        private static PermissionTextReaderSettings CreateSettings(AnalyzerConfigOptions options)
        {
            var settings = new PermissionTextReaderSettings();

            var indentString = options.GetMetadata("IndentCharacter");
            if (indentString.Length != 1)
            {
                throw new FormatException($"IndentCharacter must be a 1 character-long string{indentString}:");
            }
            else
            {
                settings.IndentCharacter = indentString[0];
            }

            settings.IndentCount = int.Parse(options.GetMetadata("IndentCount"));

            return settings;
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

            diagnostic
                .SplitMultilineDiagnostic()
                .ReportTo(context);
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

            diagnostic
                .SplitMultilineDiagnostic()
                .ReportTo(context);
        }
    }
}
