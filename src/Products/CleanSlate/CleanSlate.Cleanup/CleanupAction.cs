namespace Company.CleanSlate.Cleanup;

/// <summary>
/// Represents a single, specific file deletion action computed during a dry-run.
/// </summary>
public sealed record CleanupAction
{
    /// <summary>Gets the full, absolute file path targeted for deletion.</summary>
    public required string FilePath { get; init; }

    /// <summary>Gets the file size in bytes.</summary>
    public required long SizeInBytes { get; init; }

    /// <summary>Gets the category of this cleanup target.</summary>
    public required CleanupCategory Category { get; init; }
}
