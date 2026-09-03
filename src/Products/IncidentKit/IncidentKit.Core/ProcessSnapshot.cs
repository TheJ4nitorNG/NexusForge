namespace Company.IncidentKit.Core;

/// <summary>
/// Represents a structured snapshot of a running system process for forensic evaluation.
/// </summary>
public sealed record ProcessSnapshot
{
    /// <summary>Gets the unique system identifier of the process.</summary>
    public required int ProcessId { get; init; }

    /// <summary>Gets the name of the process.</summary>
    public required string ProcessName { get; init; }

    /// <summary>Gets the absolute executable file path of the process.</summary>
    public string? ExecutablePath { get; init; }

    /// <summary>Gets the process identifier of the parent process, if available.</summary>
    public int? ParentProcessId { get; init; }

    /// <summary>Gets the raw command-line invocation arguments of the process, if available.</summary>
    public string? CommandLine { get; init; }

    /// <summary>Gets the physical working set memory size of the process in bytes.</summary>
    public required long WorkingSetMemory { get; init; }

    /// <summary>Gets the creation or start timestamp of the process.</summary>
    public DateTimeOffset CreationTime { get; init; }

    /// <summary>Gets the collection of active modules loaded into the process's memory space.</summary>
    public IReadOnlyList<ModuleSnapshot> Modules { get; init; } = [];
}
