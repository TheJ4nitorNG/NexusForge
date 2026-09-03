namespace Company.CleanSlate.Cleanup;

/// <summary>
/// Configurations specifying what directory and categories are targeted for a scan or clean.
/// </summary>
public sealed record CleanupProfile
{
    /// <summary>Gets the target directory path to analyze and clean.</summary>
    public required string TargetDirectory { get; init; }

    /// <summary>Gets the collection of active cleanup categories.</summary>
    public IReadOnlySet<CleanupCategory> ActiveCategories { get; init; } = new HashSet<CleanupCategory>();
}
