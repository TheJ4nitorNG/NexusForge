using System.ServiceProcess;
using Company.Platform.Abstractions;

namespace Company.Platform.Services;

/// <summary>
/// Provides information and control over system services using System.ServiceProcess.
/// </summary>
public sealed class ServiceManager : IServiceManager
{
    /// <inheritdoc />
    public Task<IReadOnlyList<ServiceInfo>> GetServicesAsync(CancellationToken cancellationToken)
    {
#pragma warning disable CA1416 // Validate platform compatibility
        ServiceController[] services = ServiceController.GetServices();
        List<ServiceInfo> result = new(services.Length);

        foreach (ServiceController service in services)
        {
            string startupType = "Unknown";
            try
            {
                startupType = service.StartType.ToString();
            }
            catch
            {
                // StartType might throw if the service handle is invalid or access is denied.
            }

            result.Add(new ServiceInfo(
                service.ServiceName,
                service.DisplayName,
                service.Status.ToString(),
                startupType));

            service.Dispose();
        }

        return Task.FromResult<IReadOnlyList<ServiceInfo>>(result);
#pragma warning restore CA1416
    }
}
