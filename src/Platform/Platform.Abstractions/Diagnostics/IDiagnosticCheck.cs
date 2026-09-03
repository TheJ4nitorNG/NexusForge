namespace Company.Platform.Abstractions.Diagnostics;

/// <summary>
/// Represents a diagnostic check that can be executed to determine system health.
/// </summary>
public interface IDiagnosticCheck
{
    /// <summary>Gets the unique identifier of the check.</summary>
    string Id { get; }

    /// <summary>Gets the display name of the check.</summary>
    string Name { get; }

    /// <summary>Gets the category of the check.</summary>
    string Category { get; }

    /// <summary>
    /// Executes the diagnostic check.
    /// </summary>
    /// <param name="context">The execution context.</param>
    /// <returns>A task that represents the asynchronous operation, containing the diagnostic result.</returns>
    Task<DiagnosticResult> ExecuteAsync(DiagnosticContext context);
}
