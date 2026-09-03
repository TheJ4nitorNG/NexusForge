using System.Diagnostics;
using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Default implementation of the diagnostic coordinator that runs checks concurrently.
/// </summary>
/// <param name="checks">The collection of diagnostic checks to execute.</param>
public sealed class DiagnosticCoordinator(IEnumerable<IDiagnosticCheck> checks) : IDiagnosticCoordinator
{
    private readonly IReadOnlyList<IDiagnosticCheck> _checks = [.. checks];

    /// <inheritdoc />
    public async Task<ScanReport> RunScanAsync(string scanId, DiagnosticContext context)
    {
        DateTimeOffset startedAt = DateTimeOffset.UtcNow;
        Stopwatch stopwatch = Stopwatch.StartNew();

        IEnumerable<Task<DiagnosticResult>> tasks = _checks.Select(async check =>
        {
            try
            {
                // We wrap each check in a try-catch to ensure one failing check does not crash the coordinator.
                return await check.ExecuteAsync(context).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return new DiagnosticResult
                {
                    CheckId = check.Id,
                    CheckName = check.Name,
                    Status = DiagnosticStatus.Skipped,
                    Message = "Check was cancelled."
                };
            }
            catch (Exception ex)
            {
                return new DiagnosticResult
                {
                    CheckId = check.Id,
                    CheckName = check.Name,
                    Status = DiagnosticStatus.Error,
                    Message = "An unexpected error occurred during execution: " + ex.Message
                };
            }
        });

        DiagnosticResult[] results = await Task.WhenAll(tasks).ConfigureAwait(false);

        stopwatch.Stop();

        // Calculate a simple health score: 100 - (10 for Critical, 5 for Error, 2 for Warning)
        int healthScore = 100;
        foreach (DiagnosticResult result in results)
        {
            if (result.Status is DiagnosticStatus.Error or DiagnosticStatus.Critical)
            {
                healthScore -= 10;
            }
            else if (result.Status == DiagnosticStatus.Warning)
            {
                healthScore -= 2;
            }

            foreach (DiagnosticFinding finding in result.Findings)
            {
                if (finding.Severity == DiagnosticStatus.Critical)
                {
                    healthScore -= 10;
                }
                else if (finding.Severity == DiagnosticStatus.Error)
                {
                    healthScore -= 5;
                }
                else if (finding.Severity == DiagnosticStatus.Warning)
                {
                    healthScore -= 2;
                }
            }
        }

        healthScore = Math.Max(0, healthScore);

        return new ScanReport(
            scanId,
            startedAt,
            stopwatch.Elapsed,
            healthScore,
            results);
    }
}
