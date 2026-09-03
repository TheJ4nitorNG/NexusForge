namespace Company.CleanSlate.Duplicates;

/// <summary>
/// Defines the contract for detecting duplicate files in a directory.
/// </summary>
public interface IDuplicateDetector
{
    /// <summary>
    /// Scans a target path and finds all groups of duplicate files asynchronously.
    /// </summary>
    /// <param name="targetPath">The directory path to scan.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of discovered duplicate groups.</returns>
    Task<IReadOnlyList<DuplicateGroup>> FindDuplicatesAsync(
        string targetPath,
        CancellationToken cancellationToken);
}
