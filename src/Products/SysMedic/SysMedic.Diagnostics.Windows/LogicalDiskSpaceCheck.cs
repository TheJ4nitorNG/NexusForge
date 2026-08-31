namespace Company.SysMedic.Diagnostics.Windows;

/// <summary>
/// A diagnostic check that evaluates the free space of logical disks.
/// </summary>
public sealed class LogicalDiskSpaceCheck : IDiagnosticCheck
{
    /// <inheritdoc />
    public string Id => "windows.storage.freespace";

    /// <inheritdoc />
    public string Name => "Logical Disk Free Space";

    /// <inheritdoc />
    public DiagnosticCategory Category => DiagnosticCategory.Storage;

    /// <inheritdoc />
    public Task<DiagnosticResult> ExecuteAsync(DiagnosticContext context, CancellationToken cancellationToken)
    {
        List<DiagnosticFinding> findings = [];
        DiagnosticSeverity highestSeverity = DiagnosticSeverity.Information;

        DriveInfo[] drives = DriveInfo.GetDrives();
        foreach (DriveInfo drive in drives.Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
        {
            cancellationToken.ThrowIfCancellationRequested();

            double freePercentage = (double)drive.AvailableFreeSpace / drive.TotalSize;

            if (freePercentage < 0.05)
            {
                findings.Add(new DiagnosticFinding(
                    "DISK_CRITICAL_SPACE",
                    DiagnosticSeverity.Critical,
                    $"Critical Low Space on {drive.Name}",
                    $"Drive {drive.Name} has less than 5% free space remaining ({freePercentage * 100:F1}%).",
                    new Dictionary<string, object?> { { "DriveName", drive.Name }, { "FreeSpaceBytes", drive.AvailableFreeSpace } }));
                highestSeverity = DiagnosticSeverity.Critical;
            }
            else if (freePercentage < 0.15)
            {
                findings.Add(new DiagnosticFinding(
                    "DISK_LOW_SPACE",
                    DiagnosticSeverity.Moderate,
                    $"Low Space on {drive.Name}",
                    $"Drive {drive.Name} has less than 15% free space remaining ({freePercentage * 100:F1}%).",
                    new Dictionary<string, object?> { { "DriveName", drive.Name }, { "FreeSpaceBytes", drive.AvailableFreeSpace } }));

                if (highestSeverity < DiagnosticSeverity.Moderate)
                {
                    highestSeverity = DiagnosticSeverity.Moderate;
                }
            }
        }

        DiagnosticStatus status = highestSeverity switch
        {
            DiagnosticSeverity.Critical => DiagnosticStatus.Failed,
            DiagnosticSeverity.High => DiagnosticStatus.Warning,
            DiagnosticSeverity.Moderate => DiagnosticStatus.Warning,
            DiagnosticSeverity.Low => DiagnosticStatus.Passed,
            DiagnosticSeverity.Information => DiagnosticStatus.Passed,
            _ => DiagnosticStatus.Passed
        };

        string summary = status == DiagnosticStatus.Passed
            ? "All fixed drives have sufficient free space."
            : $"Found {findings.Count} drives with low free space.";

        return Task.FromResult(new DiagnosticResult
        {
            CheckId = Id,
            Status = status,
            Severity = highestSeverity,
            Summary = summary,
            Findings = findings
        });
    }
}
