using Company.Platform.Abstractions.Diagnostics;

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
    public string Category => "Storage";

    /// <inheritdoc />
    public System.Threading.Tasks.Task<DiagnosticResult> ExecuteAsync(DiagnosticContext context)
    {
        System.Collections.Generic.List<DiagnosticFinding> findings = [];
        DiagnosticStatus highestSeverity = DiagnosticStatus.Healthy;

        System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
        foreach (System.IO.DriveInfo drive in System.Linq.Enumerable.Where(drives, d => d.IsReady && d.DriveType == System.IO.DriveType.Fixed))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            double freePercentage = (double)drive.AvailableFreeSpace / drive.TotalSize;

            if (freePercentage < 0.05)
            {
                findings.Add(new DiagnosticFinding
                {
                    Id = "DISK_CRITICAL_SPACE",
                    Severity = DiagnosticStatus.Critical,
                    Message = $"Critical Low Space on {drive.Name}. Less than 5% free space remaining ({freePercentage * 100:F1}%).",
                    Recommendation = $"Free up space on {drive.Name} immediately to prevent system instability."
                });

                highestSeverity = DiagnosticStatus.Critical;
            }
            else if (freePercentage < 0.15)
            {
                findings.Add(new DiagnosticFinding
                {
                    Id = "DISK_LOW_SPACE",
                    Severity = DiagnosticStatus.Warning,
                    Message = $"Low Space on {drive.Name}. Less than 15% free space remaining ({freePercentage * 100:F1}%).",
                    Recommendation = $"Consider freeing up space on {drive.Name}."
                });

                if (highestSeverity != DiagnosticStatus.Critical)
                {
                    highestSeverity = DiagnosticStatus.Warning;
                }
            }
        }

        DiagnosticStatus status = highestSeverity switch
        {
            DiagnosticStatus.Critical => DiagnosticStatus.Critical,
            DiagnosticStatus.Error => DiagnosticStatus.Error,
            DiagnosticStatus.Warning => DiagnosticStatus.Warning,
            DiagnosticStatus.Healthy => DiagnosticStatus.Healthy,
            DiagnosticStatus.Skipped => DiagnosticStatus.Skipped,
            DiagnosticStatus.Unknown => DiagnosticStatus.Unknown,
            _ => DiagnosticStatus.Healthy
        };

        string summary = status == DiagnosticStatus.Healthy
            ? "All fixed drives have sufficient free space."
            : $"Found {findings.Count} drives with low free space.";

        return System.Threading.Tasks.Task.FromResult(new DiagnosticResult
        {
            CheckId = Id,
            CheckName = Name,
            Status = status,
            Message = summary,
            Findings = findings
        });
    }
}
