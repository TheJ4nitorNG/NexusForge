namespace Company.Platform.Abstractions.Diagnostics;

/// <summary>
/// Represents the result of a diagnostic check.
/// </summary>
public sealed record DiagnosticResult
{
    /// <summary>Gets the identifier of the check.</summary>
    public required string CheckId { get; init; }

    /// <summary>Gets the name of the check.</summary>
    public required string CheckName { get; init; }

    /// <summary>Gets the final status of the check.</summary>
    public required DiagnosticStatus Status { get; init; }

    /// <summary>Gets the summary message.</summary>
    public string? Message { get; init; }

    /// <summary>Gets the duration of the check.</summary>
    public TimeSpan Duration { get; init; }

    /// <summary>Gets the list of findings produced by the check.</summary>
    public IReadOnlyList<DiagnosticFinding> Findings { get; init; } = [];
}
