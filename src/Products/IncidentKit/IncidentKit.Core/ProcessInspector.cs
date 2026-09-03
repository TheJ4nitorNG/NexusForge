using System.Diagnostics;
using System.Management;
using System.Runtime.Versioning;

namespace Company.IncidentKit.Core;

/// <summary>
/// Provides secure system process and DLL module scanning capability for live forensics.
/// </summary>
[SupportedOSPlatform("windows")]
public static class ProcessInspector
{
    /// <summary>
    /// Captures a live snapshot of all running processes, their command lines, and loaded modules.
    /// </summary>
    /// <returns>A collection of forensic process snapshots.</returns>
    public static IReadOnlyList<ProcessSnapshot> CaptureActiveProcesses()
    {
        Dictionary<int, (int? ParentId, string? CommandLine, string? ExecPath)> wmiData = GetWmiProcessData();
        List<ProcessSnapshot> snapshots = [];

        foreach (Process proc in Process.GetProcesses())
        {
            _ = wmiData.TryGetValue(proc.Id, out (int? ParentId, string? CommandLine, string? ExecPath) procWmi);

            List<ModuleSnapshot> modules = [];
            try
            {
                // Accessing Process.Modules throws Win32Exception for elevated system processes
                // or x64 vs x86 architecture mismatches. We skip locked modules safely.
                foreach (ProcessModule mod in proc.Modules)
                {
                    string? filePath = mod.FileName;
                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    {
                        modules.Add(new ModuleSnapshot
                        {
                            ModuleName = mod.ModuleName ?? "Unknown",
                            FilePath = filePath,
                            FileSize = new FileInfo(filePath).Length,
                            Description = mod.FileVersionInfo?.FileDescription
                        });
                    }
                }
            }
            catch
            {
                // Safely skip modules we don't have access permissions for
            }

            snapshots.Add(new ProcessSnapshot
            {
                ProcessId = proc.Id,
                ProcessName = proc.ProcessName,
                ExecutablePath = procWmi.ExecPath ?? GetSafeMainModulePath(proc),
                ParentProcessId = procWmi.ParentId,
                CommandLine = procWmi.CommandLine,
                WorkingSetMemory = proc.WorkingSet64,
                CreationTime = GetSafeStartTime(proc),
                Modules = modules
            });
        }

        return snapshots;
    }

    private static Dictionary<int, (int? ParentId, string? CommandLine, string? ExecPath)> GetWmiProcessData()
    {
        Dictionary<int, (int? ParentId, string? CommandLine, string? ExecPath)> data = [];

        try
        {
            // Fetching all process details in a single query is extremely fast (under 50ms)
            using ManagementObjectSearcher searcher = new("SELECT ProcessId, ParentProcessId, CommandLine, ExecutablePath FROM Win32_Process");
            using ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementBaseObject obj in collection)
            {
                int processId = Convert.ToInt32(obj["ProcessId"], System.Globalization.CultureInfo.InvariantCulture);
                int? parentId = obj["ParentProcessId"] != null
                    ? Convert.ToInt32(obj["ParentProcessId"], System.Globalization.CultureInfo.InvariantCulture)
                    : null;
                string? commandLine = obj["CommandLine"] as string;
                string? execPath = obj["ExecutablePath"] as string;

                data[processId] = (parentId, commandLine, execPath);
            }
        }
        catch
        {
            // Fallback gracefully if WMI service is unavailable or corrupt
        }

        return data;
    }

    private static string? GetSafeMainModulePath(Process process)
    {
        try
        {
            return process.MainModule?.FileName;
        }
        catch
        {
            return null;
        }
    }

    private static DateTimeOffset GetSafeStartTime(Process process)
    {
        try
        {
            return process.StartTime;
        }
        catch
        {
            return DateTimeOffset.MinValue;
        }
    }
}
