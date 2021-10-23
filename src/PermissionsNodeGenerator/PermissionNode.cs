using System;
using System.Collections.Generic;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Represents a node in a permission tree.
    /// </summary>
    public class PermissionNode
    {
        /// <summary>
        /// Gets the parent of this node.
        /// </summary>
        public PermissionNode Parent { get; }

        /// <summary>
        /// Gets the name of this node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the children of this node.
        /// </summary>
        public IReadOnlyList<PermissionNode> Children { get; }

        /// <summary>
        /// Gets an empty read-only list of children.
        /// </summary>
        /// <remarks>A read-only list is immutable, therefore it can be safely reused.</remarks>
        private static readonly IReadOnlyList<PermissionNode> _emptyChildren = Array.AsReadOnly(new PermissionNode[0]);

        /// <summary>
        /// Initializes a new instance of <see cref="PermissionNode"/>.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="parent">The parent of the node.</param>
        internal PermissionNode(
            string name,
            PermissionNode parent = null,
            IReadOnlyList<PermissionNode> children = null)
        {
            Name = name;
            Parent = parent;
            Children = children ?? _emptyChildren;
        }
    }
}
