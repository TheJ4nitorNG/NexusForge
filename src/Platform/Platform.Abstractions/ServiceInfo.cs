namespace Company.Platform.Abstractions;

/// <summary>
/// Contains information about a system service.
/// </summary>
/// <param name="ServiceName">The name of the service.</param>
/// <param name="DisplayName">The display name of the service.</param>
/// <param name="Status">The current status of the service.</param>
/// <param name="StartupType">The startup type of the service.</param>
public sealed record ServiceInfo(
    string ServiceName,
    string DisplayName,
    string Status,
    string StartupType);
