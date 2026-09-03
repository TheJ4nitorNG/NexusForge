namespace Company.Platform.Abstractions.Diagnostics;

/// <summary>
/// Context passed to a diagnostic check when executed.
/// </summary>
public sealed record DiagnosticContext
{
    /// <summary>
    /// Gets the cancellation token for the operation.
    /// </summary>
    public CancellationToken CancellationToken { get; init; } = CancellationToken.None;

    /// <summary>
    /// Gets a value indicating whether the current process is elevated.
    /// </summary>
    public bool IsElevated { get; init; }
}
