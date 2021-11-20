using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace PermissionsNodeGenerator
{
    /// <summary>
    /// The exception is thrown when the indent of a line is invalid.
    /// </summary>
    [Serializable]
    public class InvalidIndentException : Exception
    {
        /// <summary>
        /// Gets the line number where the invalid indent occured.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the line position where the invalid indent occured.
        /// </summary>
        public int Position { get; }

        /// <inheritdoc cref="PermissionsTextReaderSettings.IndentCharacter"/>
        public char IndentCharacter { get; }

        /// <inheritdoc cref="PermissionsTextReaderSettings.IndentCount"/>
        public int IndentCount { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InvalidIndentException"/>.
        /// </summary>
        /// <param name="line">The line number.</param>
        /// <param name="position">The line position.</param>
        /// <param name="indentCharacter">The character used for indentation.</param>
        /// <param name="indentCount">The amount of <paramref name="indentCharacter"/> in each indent level.</param>
        public InvalidIndentException(
            int line,
            int position,
            char indentCharacter,
            int indentCount)
        {
            Line = line;
            Position = position;
            IndentCharacter = indentCharacter;
            IndentCount = indentCount;
        }

        /// <inheritdoc />
        protected InvalidIndentException(
          SerializationInfo info,
          StreamingContext context) : base(info, context)
        {
            Line = info.GetInt32(nameof(Line));
            Position = info.GetInt32(nameof(Position));
            IndentCharacter = info.GetChar(nameof(IndentCharacter));
            IndentCount = info.GetInt32(nameof(IndentCount));
        }

        /// <inheritdoc />
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue(nameof(Line), Line);
            info.AddValue(nameof(Position), Position);
            info.AddValue(nameof(IndentCharacter), IndentCharacter);
            info.AddValue(nameof(IndentCount), IndentCount);

            base.GetObjectData(info, context);
        }
    }
}
