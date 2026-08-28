namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Defines the contract for an executable diagnostic check.
/// </summary>
public interface IDiagnosticCheck
{
    /// <summary>
    /// Gets the unique identifier for this check.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the human-readable name of this check.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the category of this check.
    /// </summary>
    DiagnosticCategory Category { get; }

    /// <summary>
    /// Executes the diagnostic check asynchronously.
    /// </summary>
    /// <param name="context">The execution context.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the diagnostic result.</returns>
    Task<DiagnosticResult> ExecuteAsync(
        DiagnosticContext context,
        CancellationToken cancellationToken);
}
