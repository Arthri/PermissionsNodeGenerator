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
        public List<PermissionNode> Children { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PermissionNode"/>.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="parent">The parent of the node.</param>
        internal PermissionNode(
            string name,
            PermissionNode parent = null)
        {
            Name = name;
            Parent = parent;
            Children = new List<PermissionNode>();
        }
    }
}
