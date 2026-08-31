namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Coordinates the execution of multiple diagnostic checks.
/// </summary>
public interface IDiagnosticCoordinator
{
    /// <summary>
    /// Executes a full scan using the provided context.
    /// </summary>
    /// <param name="context">The diagnostic context for the scan.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the scan report.</returns>
    Task<ScanReport> RunScanAsync(
        DiagnosticContext context,
        CancellationToken cancellationToken);
}
