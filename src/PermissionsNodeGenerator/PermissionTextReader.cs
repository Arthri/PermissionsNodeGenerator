using System.Collections.Generic;
using System.IO;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Provides methods to read a permission text file.
    /// </summary>
    public static class PermissionTextReader
    {
        /// <summary>
        /// Determines if the specified name is valid.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns><see langword="true"/> if the name is valid, otherwise <see langword="false"/>.</returns>
        public static bool IsValidName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];

                if (c == '.')
                {
                    // Once we find an undesirable character, short circuit
                    // and return false
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines the indent count of the specified line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>The indent count.</returns>
        public static int CountIndent(string line)
        {
            // The indent count
            var i = 0;
            for (; i < line.Length; i++)
            {
                var c = line[i];
                if (c != ' ')
                {
                    break;
                }
            }

            return i;
        }

        /// <summary>
        /// Parses the specified stream as a permissions document.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An array of permissions nodes representing the parsed document.</returns>
        public static IReadOnlyList<PermissionNode> Parse(Stream stream)
        {
            var rootNode = new PermissionNode("", null);

            using (var reader = new StreamReader(stream))
            {
                // A stack representing the hierachy of nodes
                var nodeStack = new Stack<PermissionNode>();
                nodeStack.Push(rootNode);

                // The depth, also known as the indent to read for
                var depth = 0;

                // The last node parsed
                PermissionNode previousNode = null;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var indent = CountIndent(line);

                    if (indent < depth)
                    {
                        do
                        {
                            nodeStack.Pop();
                        }
                        while (indent < --depth);
                    }
                    else if (indent == depth + 1)
                    {
                        if (previousNode is null)
                        {
                            throw null;
                            // invalid format
                        }

                        nodeStack.Push(previousNode);
                        depth++;
                    }
                    else if (indent != depth)
                    {
                        throw null;
                        // invalid format
                    }

                    string name = line.Trim();

                    if (!IsValidName(name))
                    {
                        throw null;
                        // todo: invalid name
                    }

                    var parent = nodeStack.Peek();
                    var currentNode = new PermissionNode(name, parent);
                    parent.Children.Add(currentNode);

                    previousNode = currentNode;
                }
            }



            return rootNode.Children.AsReadOnly();
        }
    }
}
