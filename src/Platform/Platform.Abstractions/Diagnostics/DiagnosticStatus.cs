namespace Company.Platform.Abstractions.Diagnostics;

/// <summary>
/// Represents the status of a diagnostic check.
/// </summary>
public enum DiagnosticStatus
{
    /// <summary>Status is unknown.</summary>
    Unknown = 0,
    /// <summary>The check passed.</summary>
    Healthy = 1,
    /// <summary>The check passed but with warnings.</summary>
    Warning = 2,
    /// <summary>The check failed critically.</summary>
    Critical = 3,
    /// <summary>An error occurred executing the check.</summary>
    Error = 4,
    /// <summary>The check was skipped.</summary>
    Skipped = 5
}
