using System.Collections.Generic;
using System.Text;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Provides methods to generate permissions classes.
    /// </summary>
    public static class PermissionsClassTemplater
    {
        /// <summary>
        /// Returns the source code for a permissions class representing the specified list.
        /// </summary>
        /// <param name="list">The list</param>
        /// <returns><inheritdoc cref="GenerateTemplate(IReadOnlyList{PermissionNode})" path="/summary"/></returns>
        public static string GenerateTemplate(IReadOnlyList<PermissionNode> list, string className)
        {
            var sb = new StringBuilder();

            sb.AppendLine(
$@"namespace GeneratedPermissions
{{
    public static class {className}
    {{");

            GenerateClassOrConst(list, sb, "        ");

            sb.AppendLine(
@"    }
}");

            return sb.ToString();
        }

        /// <summary>
        /// Turns the specified nodes into either classes or consts.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sb">The <see cref="StringBuilder"/> to append to.</param>
        /// <param name="indent">The indent.</param>
        private static void GenerateClassOrConst(IReadOnlyList<PermissionNode> list, StringBuilder sb, string indent = "")
        {
            foreach (var node in list)
            {
                if (node.Children.Count == 0)
                {
                    sb.AppendLine($"{indent}public const string {node.Name} = \"{node.Path.ToLower()}\";");
                }
                else
                {
                    sb.AppendLine(
$@"{indent}public static class {node.Name}
{indent}{{");

                    GenerateClassOrConst(node.Children, sb, indent + "    ");

                    sb.AppendLine(
$"{indent}}}");
                }
            }
        }
    }
}
