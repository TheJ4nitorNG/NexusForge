namespace Company.Platform.Abstractions;

/// <summary>
/// Provides information about running processes.
/// </summary>
public interface IProcessService
{
    /// <summary>
    /// Gets the list of currently running processes asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of process information.</returns>
    Task<IReadOnlyList<ProcessInfo>> GetProcessesAsync(
        CancellationToken cancellationToken);
}
