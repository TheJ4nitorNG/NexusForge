namespace Company.CleanSlate.Duplicates;

/// <summary>
/// Represents a group of duplicate files found on the system.
/// </summary>
public sealed record DuplicateGroup
{
    /// <summary>
    /// Gets the size of each file in the group in bytes.
    /// </summary>
    public required long FileSize { get; init; }

    /// <summary>
    /// Gets the cryptographic hash of the duplicate files.
    /// </summary>
    public required string FileHash { get; init; }

    /// <summary>
    /// Gets the list of file paths that are identical duplicates.
    /// </summary>
    public required IReadOnlyList<string> FilePaths { get; init; }
}
