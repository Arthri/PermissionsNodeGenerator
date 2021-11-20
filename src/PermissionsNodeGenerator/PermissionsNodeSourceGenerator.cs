using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using PermissionsNodeGenerator.Extensions;
using PermissionsNodeGenerator.Results;
using PermissionsNodeGenerator.Results.Settings;
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

                    if (!settings.Success)
                    {
                        Diagnostic
                            .Create(
                                new DiagnosticDescriptor(
                                    "PNSG1002",
                                    "Unsuccessfully parsed settings",
                                    settings.Message,
                                    "Format",
                                    settings.Value == null
                                        ? DiagnosticSeverity.Warning
                                        : DiagnosticSeverity.Error,
                                    settings.Value == null),
                                Location.None)
                            .SplitMultilineDiagnostic()
                            .ReportTo(context);
                    }

                    if (settings.Value == null)
                    {
                        continue;
                    }

                    IReadOnlyList<PermissionNode> nodes;


                    var sourceText = file.GetText(context.CancellationToken);

                    if (sourceText is null)
                    {
                        Diagnostic
                            .Create(
                                new DiagnosticDescriptor(
                                    "PNSG0002",
                                    "Source text is null",
                                    "Source text for {0} is null",
                                    "Other",
                                    DiagnosticSeverity.Error,
                                    true),
                                Location.Create(
                                    file.Path,
                                    new TextSpan(),
                                    new LinePositionSpan()),
                                file.Path)
                            .ReportTo(context);
                        continue;
                    }

                    var lines = sourceText
                        .Lines
                        .Select(l => l.Span.ToString());

                    try
                    {
                        nodes = PermissionTextReader.Parse(lines, settings.Value);
                    }
                    catch (Exception e)
                    {
                        ReportCantParseExceptionDiagnostic(context, e, file.Path);
                        continue;
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

        private static IResult<PermissionTextReaderSettings> CreateSettings(AnalyzerConfigOptions options)
        {
            var settings = new PermissionTextReaderSettings();

            var indentString = options.GetMetadata("IndentCharacter");
            if (indentString == null || indentString.Length == 0)
            {
                // Do nothing and use default
            }
            else if (indentString.Length == 1)
            {
                settings.IndentCharacter = indentString[0];
            }
            else if (indentString.Length == 3
                  && indentString.StartsWith("'")
                  && indentString.EndsWith("'"))
            {
                // Apostrophe notation 'char', eg. ' '
                // to prevent MSBuild from trimming the space/tab character away
                settings.IndentCharacter = indentString[1];
            }
            else
            {
                return new SettingsResult
                {
                    Value = null,
                    Success = false,
                    Message = "IndentCharacter must be a 1 character-long string."
                };
            }

            var indentCountString = options.GetMetadata("IndentCount");
            if (indentCountString == null || indentCountString.Length == 0)
            {
                // Do nothing and use default
            }
            else
            {
                try
                {
                    settings.IndentCount = int.Parse(indentCountString);
                }
                catch (Exception e)
                {
                    return new SettingsResult
                    {
                        Value = null,
                        Success = false,
                        Message = $"Exception thrown while parsing {nameof(PermissionTextReaderSettings.IndentCount)}:\n{e}"
                    };
                }
            }

            return new SettingsResult
            {
                Value = settings,
                Success = true,
                Message = "Successfully parsed settings."
            };
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
