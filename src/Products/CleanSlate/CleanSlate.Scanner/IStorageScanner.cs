namespace Company.CleanSlate.Scanner;

/// <summary>
/// Provides a mechanism to scan local storage devices for files and directories.
/// </summary>
public interface IStorageScanner
{
    /// <summary>
    /// Scans the storage asynchronously based on the provided options.
    /// </summary>
    /// <param name="options">The options configuring the scan behavior.</param>
    /// <param name="progress">An optional provider for progress updates.</param>
    /// <param name="cancellationToken">A token to cancel the scan operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the scan result.</returns>
    Task<StorageScanResult> ScanAsync(
        StorageScanOptions options,
        IProgress<ScanProgress>? progress,
        CancellationToken cancellationToken);
}
