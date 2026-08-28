namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Represents the comprehensive result of executing a diagnostic check.
/// </summary>
public sealed record DiagnosticResult
{
    /// <summary>
    /// Gets the unique identifier of the check that produced this result.
    /// </summary>
    public required string CheckId { get; init; }

    /// <summary>
    /// Gets the overall execution status.
    /// </summary>
    public required DiagnosticStatus Status { get; init; }

    /// <summary>
    /// Gets the highest severity found during the check.
    /// </summary>
    public required DiagnosticSeverity Severity { get; init; }

    /// <summary>
    /// Gets a human-readable summary of the result.
    /// </summary>
    public required string Summary { get; init; }

    /// <summary>
    /// Gets additional details, if any.
    /// </summary>
    public string? Details { get; init; }

    /// <summary>
    /// Gets the list of specific findings.
    /// </summary>
    public IReadOnlyList<DiagnosticFinding> Findings { get; init; } = [];
}
