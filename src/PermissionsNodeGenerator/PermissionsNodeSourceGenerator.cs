using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
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
            var targetFiles = context
                .AdditionalFiles
                .Where(af => af.Path.EndsWith(".ptxt"));

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
                    nodes = PermissionTextReader.Parse(stream);
                }


                var className = Path.GetFileNameWithoutExtension(file.Path);
                var generatedSource = PermissionsClassTemplater.GenerateTemplate(nodes, className);

                context.AddSource(
                    $"{className}.cs",
                    SourceText.From(generatedSource, Encoding.UTF8));
            }
        }
    }
}
