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
    }
}
