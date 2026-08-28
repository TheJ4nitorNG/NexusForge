namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Represents the context provided to a diagnostic check when it executes.
/// </summary>
public sealed record DiagnosticContext
{
    /// <summary>
    /// Gets the unique identifier for the current scan operation.
    /// </summary>
    public required string ScanId { get; init; }

    /// <summary>
    /// Gets the timestamp when the scan started.
    /// </summary>
    public required DateTimeOffset StartedAt { get; init; }

    /// <summary>
    /// Gets the cancellation token for the operation.
    /// </summary>
    public required CancellationToken CancellationToken { get; init; }

    /// <summary>
    /// Gets the system snapshot to use for this scan.
    /// </summary>
    public required ISystemSnapshot Snapshot { get; init; }
}
