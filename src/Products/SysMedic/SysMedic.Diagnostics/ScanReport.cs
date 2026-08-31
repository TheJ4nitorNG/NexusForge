namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Represents the aggregated report of a full diagnostic scan.
/// </summary>
/// <param name="ScanId">The unique identifier of the scan.</param>
/// <param name="StartedAt">The time the scan started.</param>
/// <param name="Duration">The total duration of the scan.</param>
/// <param name="OverallHealthScore">The calculated health score (0-100).</param>
/// <param name="Results">The list of individual diagnostic results.</param>
public sealed record ScanReport(
    string ScanId,
    DateTimeOffset StartedAt,
    TimeSpan Duration,
    int OverallHealthScore,
    IReadOnlyList<DiagnosticResult> Results);
