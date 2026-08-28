namespace Company.Platform.Abstractions;

/// <summary>
/// Provides information and control over system services.
/// </summary>
public interface IServiceManager
{
    /// <summary>
    /// Gets a list of system services asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of service information.</returns>
    Task<IReadOnlyList<ServiceInfo>> GetServicesAsync(
        CancellationToken cancellationToken);
}
