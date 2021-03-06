using Microsoft.CodeAnalysis.CSharp;
using PermissionsNodeGenerator.Results.PTextReader;
using System.Collections.Generic;
using System.Linq;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Provides methods to read a permission text file.
    /// </summary>
    public static class PermissionsTextReader
    {
        /// <summary>
        /// Parses the specified stream as a permissions document.
        /// </summary>
        /// <param name="lines">The lines to parse.</param>
        /// <returns>An array of permissions nodes representing the parsed document.</returns>
        public static PermissionsTextResult Parse(IEnumerable<string> lines, PermissionsTextReaderSettings? settings = null)
        {
            // Settings

            if (settings == null)
            {
                settings = new();
            }

            var indentChar = settings.IndentCharacter;
            var indentCount = settings.IndentCount;



            // A stack representing the hierachy of nodes
            var nodeStack = new Stack<List<PermissionNode>>();
            nodeStack.Push(new List<PermissionNode>());

            // The depth, also known as the indent count to read for
            var depth = 0;

            PermissionNode? previousNode = null;

            int lineNumber = 0;
            foreach (var line in lines)
            {
                lineNumber += 1;

                // The indent count
                var indentCharCount = 0;
                for (; indentCharCount < line.Length; indentCharCount++)
                {
                    var c = line[indentCharCount];
                    if (c != indentChar)
                    {
                        break;
                    }
                }

                if (indentCharCount == line.Length)
                {
                    // Ignore blank lines
                    continue;
                }

                if (indentCharCount % indentCount != 0)
                {
                    return new PermissionsTextErrorResult
                    {
                        Message = "Invalid indent.",
                        Line = lineNumber,
                        LinePositionStart = indentCharCount / indentCount * indentCount,
                        LinePositionEnd = indentCharCount,
                    };
                }



                var indentLevel = indentCharCount / indentCount;
                if (indentLevel < depth)
                {
                    // Indent dropped, therefore current node(s) has ended
                    // Keeps dropping until indent matches
                    do
                    {
                        nodeStack.Pop();
                    }
                    while (indentLevel < --depth);
                }
                else if (indentLevel > depth)
                {
                    // Currently, allow only 1-depth jump
                    // If previousNode is null, then there is no parent
                    // to add to yet. Currently the only applicable case
                    // is a direct jump from 0 depth to 1 depth on the
                    // very first node of the document.
                    if (indentLevel != depth + 1 || previousNode == null)
                    {
                        return new PermissionsTextErrorResult
                        {
                            Message = "Unexpected indent level jump.",
                            Line = lineNumber,
                            LinePositionStart = (depth + 1) * indentCount,
                            LinePositionEnd = indentCharCount,
                        };
                    }

                    // Indent jumped, push children and increase depth
                    nodeStack.Push(previousNode.Children);
                    depth++;
                }

                string name = line.Substring(indentCharCount);

                if (!SyntaxFacts.IsValidIdentifier(name))
                {
                    return new PermissionsTextErrorResult
                    {
                        Message = "Permissions name is not valid.",
                        Line = lineNumber,
                        LinePositionStart = indentCharCount,
                        LinePositionEnd = line.Length,
                    };
                }

                // Search for parent node
                PermissionNode? parentNode = null;
                if (nodeStack.Count > 1)
                {
                    var childrenOfGrandparents = nodeStack.Skip(1).First();
                    parentNode = childrenOfGrandparents.First();
                }

                // Get siblings list
                var siblings = nodeStack.Peek();
                var you = new PermissionNode(
                    name,
                    parentNode!,
                    new List<PermissionNode>());

                // Append myself
                siblings.Add(you);

                previousNode = you;
            }



            // Pop all except root
            while (nodeStack.Count > 1)
            {
                nodeStack.Pop();
            }

            // Return root collection
            return new PermissionsTextResult(true)
            {
                Value = nodeStack.Peek(),
                Message = "Permissions text successfully read.",
            };
        }
    }
}
