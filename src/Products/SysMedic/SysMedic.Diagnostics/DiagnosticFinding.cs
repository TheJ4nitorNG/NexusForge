namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Represents a specific finding discovered by a diagnostic check.
/// </summary>
/// <param name="Code">The stable finding code.</param>
/// <param name="Severity">The severity of the finding.</param>
/// <param name="Title">A human-readable title.</param>
/// <param name="Description">A detailed description of the finding.</param>
/// <param name="Metadata">Additional structured metadata.</param>
public sealed record DiagnosticFinding(
    string Code,
    DiagnosticSeverity Severity,
    string Title,
    string Description,
    IReadOnlyDictionary<string, object?> Metadata);
