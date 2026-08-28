namespace Company.CleanSlate.Scanner;

/// <summary>
/// Represents the final result of a storage scan operation.
/// </summary>
/// <param name="TotalFiles">The total number of files discovered.</param>
/// <param name="TotalDirectories">The total number of directories traversed.</param>
/// <param name="TotalBytes">The total size of all scanned files in bytes.</param>
/// <param name="ScanDuration">The duration of the scan operation.</param>
public sealed record StorageScanResult(
    long TotalFiles,
    long TotalDirectories,
    long TotalBytes,
    TimeSpan ScanDuration);
