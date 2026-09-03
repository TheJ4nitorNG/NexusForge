using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Company.Platform.Abstractions;

namespace Company.Platform.Windows;

/// <summary>
/// Provides system information using native .NET and OS APIs.
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class SystemInformationProvider : ISystemInformationProvider, IDisposable
{
    private SystemInformation? _cachedInformation;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <inheritdoc />
    public async Task<SystemInformation> GetSystemInformationAsync(CancellationToken cancellationToken)
    {
        if (_cachedInformation != null)
        {
            return _cachedInformation with { SystemUptime = GetUptime() };
        }

        await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_cachedInformation != null)
            {
                return _cachedInformation with { SystemUptime = GetUptime() };
            }

            string osVersion = RuntimeInformation.OSDescription;
            string architecture = RuntimeInformation.OSArchitecture.ToString();
            string machineName = Environment.MachineName;

            string cpuName = "Unknown CPU";
            ulong memory = 0;

            try
            {
                (cpuName, memory) = await Task.Run(() =>
                {
                    string cpu = "Unknown CPU";
                    ulong mem = 0;

                    using (ManagementObjectSearcher searcher = new("SELECT Name FROM Win32_Processor"))
                    {
                        foreach (ManagementBaseObject obj in searcher.Get())
                        {
                            cpu = obj["Name"]?.ToString() ?? cpu;
                            break;
                        }
                    }

                    using (ManagementObjectSearcher searcher = new("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem"))
                    {
                        foreach (ManagementBaseObject obj in searcher.Get())
                        {
                            if (ulong.TryParse(obj["TotalPhysicalMemory"]?.ToString(), out ulong memVal))
                            {
                                mem = memVal;
                            }
                            break;
                        }
                    }

                    return (cpu, mem);
                }, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                // In a real system, we'd log this. For now, we fall back gracefully.
            }

            _cachedInformation = new SystemInformation(
                osVersion,
                architecture,
                machineName,
                cpuName,
                memory,
                TimeSpan.Zero);

            return _cachedInformation with { SystemUptime = GetUptime() };
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    private static TimeSpan GetUptime()
    {
        return TimeSpan.FromMilliseconds(Environment.TickCount64);
    }

    /// <summary>
    /// Disposes resources.
    /// </summary>
    public void Dispose()
    {
        _semaphore.Dispose();
    }
}
