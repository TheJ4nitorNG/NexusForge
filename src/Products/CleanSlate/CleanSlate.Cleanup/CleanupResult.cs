namespace Company.CleanSlate.Cleanup;

/// <summary>
/// Represents the result of a completed cleanup operation.
/// </summary>
public sealed record CleanupResult
{
    /// <summary>Gets the total number of files attempted to be deleted.</summary>
    public required int FilesAttempted { get; init; }

    /// <summary>Gets the total number of files successfully deleted.</summary>
    public required int FilesDeleted { get; init; }

    /// <summary>Gets the total disk space reclaimed in bytes.</summary>
    public required long BytesReclaimed { get; init; }

    /// <summary>Gets the list of file paths that failed deletion due to locks or errors.</summary>
    public IReadOnlyList<string> FailedDeletions { get; init; } = [];
}
