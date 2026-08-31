using Company.Platform.Abstractions;

namespace Company.SysMedic.Diagnostics.Windows;

/// <summary>
/// A diagnostic check that ensures critical Windows services are running.
/// </summary>
/// <param name="serviceManager">The service manager dependency.</param>
public sealed class CriticalServicesCheck(IServiceManager serviceManager) : IDiagnosticCheck
{
    private readonly IServiceManager _serviceManager = serviceManager;

    // A list of basic critical services that generally should be running on Windows 10/11.
    private static readonly string[] CriticalServiceNames = ["Winmgmt", "EventLog", "RpcSs"];

    /// <inheritdoc />
    public string Id => "windows.services.critical";

    /// <inheritdoc />
    public string Name => "Critical Windows Services";

    /// <inheritdoc />
    public DiagnosticCategory Category => DiagnosticCategory.Services;

    /// <inheritdoc />
    public async Task<DiagnosticResult> ExecuteAsync(DiagnosticContext context, CancellationToken cancellationToken)
    {
        IReadOnlyList<ServiceInfo> services = await _serviceManager.GetServicesAsync(cancellationToken).ConfigureAwait(false);

        List<DiagnosticFinding> findings = [];
        DiagnosticSeverity highestSeverity = DiagnosticSeverity.Information;

        foreach (string criticalService in CriticalServiceNames)
        {
            ServiceInfo? serviceInfo = services.FirstOrDefault(s => s.ServiceName.Equals(criticalService, StringComparison.OrdinalIgnoreCase));

            if (serviceInfo == null)
            {
                findings.Add(new DiagnosticFinding(
                    "SERVICE_MISSING",
                    DiagnosticSeverity.Critical,
                    $"Critical Service Missing: {criticalService}",
                    $"The service '{criticalService}' is not installed on this system.",
                    new Dictionary<string, object?> { { "ServiceName", criticalService } }));
                highestSeverity = DiagnosticSeverity.Critical;
            }
            else if (!serviceInfo.Status.Equals("Running", StringComparison.OrdinalIgnoreCase))
            {
                findings.Add(new DiagnosticFinding(
                    "SERVICE_STOPPED",
                    DiagnosticSeverity.High,
                    $"Critical Service Stopped: {criticalService}",
                    $"The service '{criticalService}' is currently in state '{serviceInfo.Status}'.",
                    new Dictionary<string, object?> { { "ServiceName", criticalService }, { "Status", serviceInfo.Status } }));

                if (highestSeverity < DiagnosticSeverity.High)
                {
                    highestSeverity = DiagnosticSeverity.High;
                }
            }
        }

        DiagnosticStatus status = highestSeverity switch
        {
            DiagnosticSeverity.Critical => DiagnosticStatus.Failed,
            DiagnosticSeverity.High => DiagnosticStatus.Failed,
            DiagnosticSeverity.Moderate => DiagnosticStatus.Warning,
            DiagnosticSeverity.Low => DiagnosticStatus.Passed,
            DiagnosticSeverity.Information => DiagnosticStatus.Passed,
            _ => DiagnosticStatus.Passed
        };

        string summary = status == DiagnosticStatus.Passed
            ? "All critical Windows services are running."
            : $"Found {findings.Count} critical service issues.";

        return new DiagnosticResult
        {
            CheckId = Id,
            Status = status,
            Severity = highestSeverity,
            Summary = summary,
            Findings = findings
        };
    }
}
