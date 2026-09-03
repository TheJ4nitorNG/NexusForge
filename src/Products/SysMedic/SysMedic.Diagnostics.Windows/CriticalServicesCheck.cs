using Company.Platform.Abstractions;
using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics.Windows;

/// <summary>
/// A diagnostic check that ensures critical Windows services are running.
/// </summary>
/// <param name="serviceManager">The service manager dependency.</param>
public sealed class CriticalServicesCheck(IServiceManager serviceManager) : IDiagnosticCheck
{
    private readonly IServiceManager _serviceManager = serviceManager;

    private static readonly string[] CriticalServiceNames = ["Winmgmt", "EventLog", "RpcSs"];

    /// <inheritdoc />
    public string Id => "windows.services.critical";

    /// <inheritdoc />
    public string Name => "Critical Windows Services";

    /// <inheritdoc />
    public string Category => "Services";

    /// <inheritdoc />
    public async System.Threading.Tasks.Task<DiagnosticResult> ExecuteAsync(DiagnosticContext context)
    {
        System.Collections.Generic.IReadOnlyList<ServiceInfo> services = await _serviceManager.GetServicesAsync(context.CancellationToken).ConfigureAwait(false);

        System.Collections.Generic.List<DiagnosticFinding> findings = [];
        DiagnosticStatus highestSeverity = DiagnosticStatus.Healthy;

        foreach (string criticalService in CriticalServiceNames)
        {
            ServiceInfo? serviceInfo = System.Linq.Enumerable.FirstOrDefault(services, s => s.ServiceName.Equals(criticalService, System.StringComparison.OrdinalIgnoreCase));

            if (serviceInfo == null)
            {
                findings.Add(new DiagnosticFinding
                {
                    Id = "SERVICE_MISSING",
                    Severity = DiagnosticStatus.Critical,
                    Message = $"Critical Service Missing: The service '{criticalService}' is not installed on this system.",
                    Recommendation = "Ensure core Windows services are not disabled by unauthorized optimization tools."
                });

                highestSeverity = DiagnosticStatus.Critical;
            }
            else if (!serviceInfo.Status.Equals("Running", System.StringComparison.OrdinalIgnoreCase))
            {
                findings.Add(new DiagnosticFinding
                {
                    Id = "SERVICE_STOPPED",
                    Severity = DiagnosticStatus.Error,
                    Message = $"Critical Service Stopped: The service '{criticalService}' is currently in state '{serviceInfo.Status}'.",
                    Recommendation = $"Start the '{criticalService}' service."
                });

                if (highestSeverity != DiagnosticStatus.Critical)
                {
                    highestSeverity = DiagnosticStatus.Error;
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
            ? "All critical Windows services are running."
            : $"Found {findings.Count} critical service issues.";

        return new DiagnosticResult
        {
            CheckId = Id,
            CheckName = Name,
            Status = status,
            Message = summary,
            Findings = findings
        };
    }
}
