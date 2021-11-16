using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Provides methods to read a permission text file.
    /// </summary>
    public static class PermissionTextReader
    {
        /// <summary>
        /// Parses the specified stream as a permissions document.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An array of permissions nodes representing the parsed document.</returns>
        public static List<PermissionNode> Parse(Stream stream)
        {
            // A stack representing the hierachy of nodes
            var nodeStack = new Stack<List<PermissionNode>>();
            nodeStack.Push(new List<PermissionNode>());

            // The depth, also known as the indent count to read for
            var depth = 0;

            PermissionNode previousNode = null;

            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // The indent count
                    var indent = 0;
                    for (; indent < line.Length; indent++)
                    {
                        var c = line[indent];
                        if (c != ' ')
                        {
                            break;
                        }
                    }

                    if (indent < depth)
                    {
                        // Indent dropped, therefore current node(s) has ended
                        // Keeps dropping until indent matches
                        do
                        {
                            nodeStack.Pop();
                        }
                        while (indent < --depth);
                    }
                    else if (indent > depth)
                    {
                        // Currently, allow only 1-depth jump
                        // If previousNode is null, then there is no parent
                        // to add to yet. Currently the only applicable case
                        // is a direct jump from 0 depth to 1 depth on the
                        // very first node of the document.
                        if (indent != depth + 1 || previousNode == null)
                        {
                            throw new UnexpectedDepthJumpException();
                        }

                        // Indent jumped, push children and increase depth
                        nodeStack.Push(previousNode.Children);
                        depth++;
                    }

                    // Trim excess space
                    // TODO: factor to CountIndent possibly
                    string name = line.Trim();

                    if (!SyntaxFacts.IsValidIdentifier(name))
                    {
                        throw new InvalidNameException(name);
                    }

                    // Search for parent node
                    PermissionNode parentNode = null;
                    if (nodeStack.Count > 1)
                    {
                        var childrenOfGrandparents = nodeStack.Skip(1).First();
                        parentNode = childrenOfGrandparents.First();
                    }

                    // Get siblings list
                    var siblings = nodeStack.Peek();
                    var you = new PermissionNode(
                        name,
                        parentNode,
                        new List<PermissionNode>());

                    // Append myself
                    siblings.Add(you);

                    previousNode = you;
                }
            }



            // Pop all except root
            while (nodeStack.Count > 1)
            {
                nodeStack.Pop();
            }

            // Return root collection
            return nodeStack.Peek();
        }
    }
}
