using System.Diagnostics;

namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Default implementation of the diagnostic coordinator that runs checks concurrently.
/// </summary>
/// <param name="checks">The collection of diagnostic checks to execute.</param>
public sealed class DiagnosticCoordinator(IEnumerable<IDiagnosticCheck> checks) : IDiagnosticCoordinator
{
    private readonly IReadOnlyList<IDiagnosticCheck> _checks = [.. checks];

    /// <inheritdoc />
    public async Task<ScanReport> RunScanAsync(DiagnosticContext context, CancellationToken cancellationToken)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        IEnumerable<Task<DiagnosticResult>> tasks = _checks.Select(async check =>
        {
            try
            {
                // We wrap each check in a try-catch to ensure one failing check does not crash the coordinator.
                return await check.ExecuteAsync(context, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                return new DiagnosticResult
                {
                    CheckId = check.Id,
                    Status = DiagnosticStatus.Skipped,
                    Severity = DiagnosticSeverity.Information,
                    Summary = "Check was cancelled."
                };
            }
            catch (Exception ex)
            {
                return new DiagnosticResult
                {
                    CheckId = check.Id,
                    Status = DiagnosticStatus.Error,
                    Severity = DiagnosticSeverity.Critical,
                    Summary = "An unexpected error occurred during execution.",
                    Details = ex.Message
                };
            }
        });

        DiagnosticResult[] results = await Task.WhenAll(tasks).ConfigureAwait(false);

        stopwatch.Stop();

        // Calculate a simple health score: 100 - (10 for each critical, 5 for each high, 2 for each moderate, 1 for low)
        int healthScore = 100;
        foreach (DiagnosticResult result in results)
        {
            if (result.Status is DiagnosticStatus.Error or DiagnosticStatus.Failed)
            {
                healthScore -= result.Severity switch
                {
                    DiagnosticSeverity.Critical => 10,
                    DiagnosticSeverity.High => 5,
                    DiagnosticSeverity.Moderate => 2,
                    DiagnosticSeverity.Low => 1,
                    DiagnosticSeverity.Information => 0,
                    _ => 0
                };
            }
        }

        healthScore = Math.Max(0, healthScore);

        return new ScanReport(
            context.ScanId,
            context.StartedAt,
            stopwatch.Elapsed,
            healthScore,
            results);
    }
}
