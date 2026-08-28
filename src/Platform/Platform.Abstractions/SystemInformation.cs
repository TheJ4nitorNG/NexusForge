namespace Company.Platform.Abstractions;

/// <summary>
/// Contains system-level information.
/// </summary>
/// <param name="OsVersion">The operating system version.</param>
/// <param name="Architecture">The system architecture.</param>
/// <param name="MachineName">The machine name.</param>
public sealed record SystemInformation(
    string OsVersion,
    string Architecture,
    string MachineName);
