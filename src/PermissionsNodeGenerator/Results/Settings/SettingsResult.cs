namespace PermissionsNodeGenerator.Results.Settings
{
    /// <summary>
    /// Represents the result of parsing MSBuild metadata to <see cref="PermissionsTextReaderSettings"/>.
    /// </summary>
    public record SettingsResult : IResult<PermissionsTextReaderSettings>
    {
        /// <inheritdoc />
        public PermissionsTextReaderSettings? Value { get; init; }

        /// <inheritdoc />
        object? IResult.Value => Value;

        /// <inheritdoc />
        public bool Success { get; init; }

        /// <inheritdoc />
        public string Message { get; init; } = "";
    }
}
