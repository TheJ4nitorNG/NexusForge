namespace Company.Platform.Abstractions.Diagnostics;

/// <summary>
/// Represents a specific finding from a diagnostic check.
/// </summary>
public sealed record DiagnosticFinding
{
    /// <summary>Gets the unique identifier of the finding.</summary>
    public required string Id { get; init; }

    /// <summary>Gets the message detailing the finding.</summary>
    public required string Message { get; init; }

    /// <summary>Gets the severity of the finding.</summary>
    public required DiagnosticStatus Severity { get; init; }

    /// <summary>Gets the recommendation to resolve the finding, if any.</summary>
    public string? Recommendation { get; init; }
}
