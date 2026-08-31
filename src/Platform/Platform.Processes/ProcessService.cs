using System.Diagnostics;
using Company.Platform.Abstractions;

namespace Company.Platform.Processes;

/// <summary>
/// Provides information about running processes using System.Diagnostics.
/// </summary>
public sealed class ProcessService : IProcessService
{
    /// <inheritdoc />
    public Task<IReadOnlyList<ProcessInfo>> GetProcessesAsync(CancellationToken cancellationToken)
    {
        Process[] processes = Process.GetProcesses();
        List<ProcessInfo> result = new(processes.Length);

        foreach (Process process in processes)
        {
            // Path and CommandLine might not be accessible due to permissions,
            // so we handle exceptions appropriately.
            string? path = null;
            string? commandLine = null;

            try
            {
                // Note: Getting full path or command line of elevated processes
                // will fail if the current process is not elevated.
                path = process.MainModule?.FileName;
            }
            catch
            {
                // Access denied or process exited
            }

            result.Add(new ProcessInfo(
                process.Id,
                process.ProcessName,
                path,
                commandLine));

            process.Dispose();
        }

        return Task.FromResult<IReadOnlyList<ProcessInfo>>(result);
    }
}
