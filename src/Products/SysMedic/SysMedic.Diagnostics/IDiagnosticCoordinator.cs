using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Coordinates the execution of multiple diagnostic checks.
/// </summary>
public interface IDiagnosticCoordinator
{
    /// <summary>
    /// Executes a full scan using the provided context.
    /// </summary>
    /// <param name="scanId">The unique identifier for the scan.</param>
    /// <param name="context">The diagnostic context for the scan.</param>
    /// <returns>A task representing the asynchronous operation, returning the scan report.</returns>
    Task<ScanReport> RunScanAsync(
        string scanId,
        DiagnosticContext context);
}
