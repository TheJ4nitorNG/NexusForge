namespace Company.CleanSlate.Scanner;

/// <summary>
/// Specifies the options for configuring a storage scan.
/// </summary>
public sealed record StorageScanOptions
{
    /// <summary>
    /// Gets the starting directory path for the scan.
    /// </summary>
    public required string TargetPath { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include system files in the scan.
    /// </summary>
    public bool IncludeSystemFiles { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include hidden files in the scan.
    /// </summary>
    public bool IncludeHiddenFiles { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include protected paths in the scan.
    /// </summary>
    public bool IncludeProtectedPaths { get; init; }

    /// <summary>
    /// Gets the minimum file size in bytes to include in the scan results.
    /// </summary>
    public long MinimumFileSize { get; init; }

    /// <summary>
    /// Gets a value indicating whether to calculate cryptographic hashes for files.
    /// </summary>
    public bool CalculateHashes { get; init; }
}
