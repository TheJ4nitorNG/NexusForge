namespace Company.CleanSlate.Cleanup;

/// <summary>
/// Defines the contract for dry-running previews and executing safe cleanups on the filesystem.
/// </summary>
public interface ICleanupEngine
{
    /// <summary>
    /// Evaluates the target directory and previews what files would be safely deleted.
    /// </summary>
    /// <param name="profile">The configured cleanup profile.</param>
    /// <param name="cancellationToken">A token to cancel the preview operation.</param>
    /// <returns>A list of computed cleanup actions representing candidates for deletion.</returns>
    Task<IReadOnlyList<CleanupAction>> PreviewCleanupAsync(
        CleanupProfile profile,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the deletion of the selected cleanup actions, validating safety boundaries.
    /// </summary>
    /// <param name="actions">The list of cleanup actions to execute.</param>
    /// <param name="cancellationToken">A token to cancel the execution operation.</param>
    /// <returns>A summary result detailing the success of the cleanup operation.</returns>
    Task<CleanupResult> ExecuteCleanupAsync(
        IReadOnlyList<CleanupAction> actions,
        CancellationToken cancellationToken);
}
