namespace PermissionsNodeGenerator
{
    /// <summary>
    /// Represents settings for <see cref="PermissionTextReader"/>.
    /// </summary>
    public class PermissionTextReaderSettings
    {
        /// <summary>
        /// Gets the character used for indentation.
        /// </summary>
        public char IndentCharacter { get; set; } = ' ';

        /// <summary>
        /// Gets the amount of <see cref="IndentCharacter"/> in an indent level.
        /// </summary>
        public int IndentCount { get; set; } = 4;
    }
}
