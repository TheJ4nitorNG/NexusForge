namespace Company.Platform.Abstractions;

/// <summary>
/// Contains system-level information.
/// </summary>
/// <param name="OsVersion">The operating system version.</param>
/// <param name="Architecture">The system architecture.</param>
/// <param name="MachineName">The machine name.</param>
/// <param name="CpuName">The name of the processor.</param>
/// <param name="TotalPhysicalMemoryBytes">The total physical memory in bytes.</param>
/// <param name="SystemUptime">The duration the system has been running.</param>
public sealed record SystemInformation(
    string OsVersion,
    string Architecture,
    string MachineName,
    string CpuName,
    ulong TotalPhysicalMemoryBytes,
    TimeSpan SystemUptime);
