using System.Security.Cryptography;

namespace Company.CleanSlate.Duplicates;

/// <summary>
/// High-performance duplicate file detector using progressive hashing.
/// </summary>
public sealed class DuplicateDetector : IDuplicateDetector
{
    private const int HeaderSize = 4096; // Read first 4KB for quick validation

    /// <inheritdoc />
    public async Task<IReadOnlyList<DuplicateGroup>> FindDuplicatesAsync(
        string targetPath,
        CancellationToken cancellationToken)
    {
        return string.IsNullOrWhiteSpace(targetPath)
            ? throw new ArgumentException("Target path cannot be null or empty.", nameof(targetPath))
            : !Directory.Exists(targetPath)
                ? throw new DirectoryNotFoundException($"The target directory '{targetPath}' was not found.")
                : await Task.Run(() => FindDuplicatesInternal(targetPath, cancellationToken), cancellationToken).ConfigureAwait(false);
    }

    private static List<DuplicateGroup> FindDuplicatesInternal(
        string targetPath,
        CancellationToken cancellationToken)
    {
        // Step 1: Scan and group all files by size (files with unique sizes cannot be duplicates)
        List<FileInfo> allFiles = [];
        ScanDirectory(new DirectoryInfo(targetPath), allFiles, cancellationToken);

        List<IGrouping<long, FileInfo>> filesBySize = [.. allFiles
            .GroupBy(f => f.Length)
            .Where(g => g.Count() > 1)]; // Only keep groups with multiple files of the exact same size

        List<DuplicateGroup> duplicateGroups = [];

        foreach (IGrouping<long, FileInfo> sizeGroup in filesBySize)
        {
            cancellationToken.ThrowIfCancellationRequested();

            long fileSize = sizeGroup.Key;

            // Zero-byte files are technically "identical" but typically skipped or handled as a single special case.
            // We ignore zero-byte files to prevent false duplication alerts on empty placeholders.
            if (fileSize == 0)
            {
                continue;
            }

            // Step 2: For candidate size-groups, perform quick header hashing (first 4KB)
            Dictionary<string, List<FileInfo>> filesByHeader = [];

            foreach (FileInfo file in sizeGroup)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string? headerHash = ComputeHeaderHash(file);
                if (headerHash == null)
                {
                    continue; // Skip files we failed to read
                }

                if (!filesByHeader.TryGetValue(headerHash, out List<FileInfo>? fileList))
                {
                    fileList = [];
                    filesByHeader[headerHash] = fileList;
                }
                fileList.Add(file);
            }

            // Step 3: For candidate header-groups, compute the full file SHA256 hash to confirm identical matches
            foreach (KeyValuePair<string, List<FileInfo>> headerGroup in filesByHeader.Where(g => g.Value.Count > 1))
            {
                cancellationToken.ThrowIfCancellationRequested();

                Dictionary<string, List<FileInfo>> filesByFullHash = [];

                foreach (FileInfo file in headerGroup.Value)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string? fullHash = ComputeFullHash(file);
                    if (fullHash == null)
                    {
                        continue; // Skip if we failed to read fully
                    }

                    if (!filesByFullHash.TryGetValue(fullHash, out List<FileInfo>? fileList))
                    {
                        fileList = [];
                        filesByFullHash[fullHash] = fileList;
                    }
                    fileList.Add(file);
                }

                // Step 4: Add verified duplicates to the final report
                foreach (KeyValuePair<string, List<FileInfo>> verifiedGroup in filesByFullHash.Where(g => g.Value.Count > 1))
                {
                    duplicateGroups.Add(new DuplicateGroup
                    {
                        FileSize = fileSize,
                        FileHash = verifiedGroup.Key,
                        FilePaths = [.. verifiedGroup.Value.Select(f => f.FullName)]
                    });
                }
            }
        }

        return duplicateGroups;
    }

    private static void ScanDirectory(
        DirectoryInfo directory,
        List<FileInfo> files,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Skip reparse points and symlinks to prevent infinite loops
        if (directory.Attributes.HasFlag(FileAttributes.ReparsePoint))
        {
            return;
        }

        try
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (file.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    continue;
                }

                files.Add(file);
            }

            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                ScanDirectory(subDir, files, cancellationToken);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Safely skip unaccessible system/user folders
        }
        catch (Exception)
        {
            // Ignore temporary path or length errors
        }
    }

    private static string? ComputeHeaderHash(FileInfo file)
    {
        try
        {
            using FileStream stream = file.OpenRead();
            byte[] buffer = new byte[HeaderSize];
            int bytesRead = stream.Read(buffer, 0, HeaderSize);

            if (bytesRead == 0)
            {
                return null;
            }

            byte[] hashBytes = SHA256.HashData(buffer.AsSpan(0, bytesRead));
            return Convert.ToHexString(hashBytes);
        }
        catch
        {
            return null; // Gracefully handle locked or permission-denied files
        }
    }

    private static string? ComputeFullHash(FileInfo file)
    {
        try
        {
            using FileStream stream = new(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            byte[] hashBytes = SHA256.HashData(stream);
            return Convert.ToHexString(hashBytes);
        }
        catch
        {
            return null;
        }
    }
}
