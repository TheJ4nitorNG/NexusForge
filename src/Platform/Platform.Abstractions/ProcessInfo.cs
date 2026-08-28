namespace Company.Platform.Abstractions;

/// <summary>
/// Contains information about a running process.
/// </summary>
/// <param name="ProcessId">The process ID.</param>
/// <param name="Name">The name of the process.</param>
/// <param name="Path">The executable path, if available.</param>
/// <param name="CommandLine">The command line arguments, if available.</param>
public sealed record ProcessInfo(
    int ProcessId,
    string Name,
    string? Path,
    string? CommandLine);
