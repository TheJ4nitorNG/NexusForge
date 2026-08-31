using System.Runtime.InteropServices;
using Company.Platform.Abstractions;

namespace Company.Platform.Windows;

/// <summary>
/// Provides system information using native .NET and OS APIs.
/// </summary>
public sealed class SystemInformationProvider : ISystemInformationProvider
{
    /// <inheritdoc />
    public Task<SystemInformation> GetSystemInformationAsync(CancellationToken cancellationToken)
    {
        string osVersion = RuntimeInformation.OSDescription;
        string architecture = RuntimeInformation.OSArchitecture.ToString();
        string machineName = Environment.MachineName;

        SystemInformation result = new(
            osVersion,
            architecture,
            machineName);

        return Task.FromResult(result);
    }
}
