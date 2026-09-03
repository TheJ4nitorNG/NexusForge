namespace Company.IncidentKit.Core;

/// <summary>
/// Represents a snapshot of a loaded process module (DLL) in process memory.
/// </summary>
public sealed record ModuleSnapshot
{
    /// <summary>Gets the name of the module.</summary>
    public required string ModuleName { get; init; }

    /// <summary>Gets the absolute file path of the module on disk.</summary>
    public required string FilePath { get; init; }

    /// <summary>Gets the size of the module file in bytes.</summary>
    public required long FileSize { get; init; }

    /// <summary>Gets the description or metadata of the module, if available.</summary>
    public string? Description { get; init; }
}
