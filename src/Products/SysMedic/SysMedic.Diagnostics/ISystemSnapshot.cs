namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Provides a stable snapshot of system information that can be shared across multiple diagnostic checks.
/// </summary>
public interface ISystemSnapshot
{
    /// <summary>
    /// Gets the Windows version.
    /// </summary>
    string WindowsVersion { get; }

    /// <summary>
    /// Gets the Windows build number.
    /// </summary>
    string BuildNumber { get; }

    /// <summary>
    /// Gets the system architecture.
    /// </summary>
    string Architecture { get; }

    /// <summary>
    /// Gets the CPU identifier.
    /// </summary>
    string Cpu { get; }

    /// <summary>
    /// Gets the total system RAM in bytes.
    /// </summary>
    long TotalRamBytes { get; }
}
