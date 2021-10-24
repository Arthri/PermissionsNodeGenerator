using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// The exception that is thrown when a permission name is invalid.
    /// </summary>
    [Serializable]
    public class InvalidNameException : Exception
    {
        /// <summary>
        /// Gets the invalid name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InvalidNameException"/>.
        /// </summary>
        /// <param name="name">The invalid name.</param>
        public InvalidNameException(string name) : base("Invalid permission name")
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <inheritdoc />
        protected InvalidNameException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
            Name = info.GetString(nameof(Name));
        }

        /// <inheritdoc />
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Name), Name);

            base.GetObjectData(info, context);
        }
    }
}
