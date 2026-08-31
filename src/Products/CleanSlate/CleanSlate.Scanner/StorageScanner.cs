using System.Diagnostics;

namespace Company.CleanSlate.Scanner;

/// <summary>
/// A robust and safe scanner for traversing the file system.
/// </summary>
public sealed class StorageScanner : IStorageScanner
{
    /// <inheritdoc />
    public Task<StorageScanResult> ScanAsync(
        StorageScanOptions options,
        IProgress<ScanProgress>? progress,
        CancellationToken cancellationToken)
    {
        return Task.Run(() => ScanInternal(options, progress, cancellationToken), cancellationToken);
    }

    private static StorageScanResult ScanInternal(
        StorageScanOptions options,
        IProgress<ScanProgress>? progress,
        CancellationToken cancellationToken)
    {
        long totalFiles = 0;
        long totalDirectories = 0;
        long totalBytes = 0;

        Stopwatch stopwatch = Stopwatch.StartNew();

        DirectoryInfo rootDir = new(options.TargetPath);
        if (!rootDir.Exists)
        {
            throw new DirectoryNotFoundException($"The target directory '{options.TargetPath}' was not found.");
        }

        ScanDirectory(rootDir, options, progress, cancellationToken, ref totalFiles, ref totalDirectories, ref totalBytes);

        stopwatch.Stop();

        return new StorageScanResult(
            totalFiles,
            totalDirectories,
            totalBytes,
            stopwatch.Elapsed);
    }

    private static void ScanDirectory(
        DirectoryInfo directory,
        StorageScanOptions options,
        IProgress<ScanProgress>? progress,
        CancellationToken cancellationToken,
        ref long totalFiles,
        ref long totalDirectories,
        ref long totalBytes)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Skip reparse points and symlinks to prevent infinite loops
        if (directory.Attributes.HasFlag(FileAttributes.ReparsePoint))
        {
            return;
        }

        if (!options.IncludeHiddenFiles && directory.Attributes.HasFlag(FileAttributes.Hidden))
        {
            return;
        }

        if (!options.IncludeSystemFiles && directory.Attributes.HasFlag(FileAttributes.System))
        {
            return;
        }

        totalDirectories++;
        progress?.Report(new ScanProgress(totalFiles, totalDirectories, totalBytes, directory.FullName));

        FileSystemInfo[] fileSystemInfos;
        try
        {
            fileSystemInfos = directory.GetFileSystemInfos();
        }
        catch (UnauthorizedAccessException)
        {
            // Safely skip directories we don't have access to
            return;
        }
        catch (Exception)
        {
            // Skip other potential errors like PathTooLongException
            return;
        }

        foreach (FileSystemInfo info in fileSystemInfos)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (info is DirectoryInfo subDir)
            {
                ScanDirectory(subDir, options, progress, cancellationToken, ref totalFiles, ref totalDirectories, ref totalBytes);
            }
            else if (info is FileInfo fileInfo)
            {
                if (fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    continue; // Skip symlinked files
                }

                if (!options.IncludeHiddenFiles && fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    continue;
                }

                if (!options.IncludeSystemFiles && fileInfo.Attributes.HasFlag(FileAttributes.System))
                {
                    continue;
                }

                long size = fileInfo.Length;
                if (size < options.MinimumFileSize)
                {
                    continue;
                }

                totalFiles++;
                totalBytes += size;
            }
        }
    }
}
