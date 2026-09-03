namespace Company.CleanSlate.Cleanup;

/// <summary>
/// A highly secure operating system cleanup engine with built-in protection rules.
/// </summary>
public sealed class CleanupEngine : ICleanupEngine
{
    private static readonly string[] TempIndicators = ["temp", "tmp"];
    private static readonly string[] CacheIndicators = ["cache", "cached"];
    private static readonly string[] LogExtensions = [".log", ".bak", ".dmp", ".tmp", ".temp"];

    /// <inheritdoc />
    public async Task<IReadOnlyList<CleanupAction>> PreviewCleanupAsync(
        CleanupProfile profile,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profile);

        return string.IsNullOrWhiteSpace(profile.TargetDirectory) || !Directory.Exists(profile.TargetDirectory)
            ? throw new DirectoryNotFoundException($"The target directory '{profile.TargetDirectory}' was not found.")
            : await Task.Run(() => PreviewCleanupInternal(profile, cancellationToken), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<CleanupResult> ExecuteCleanupAsync(
        IReadOnlyList<CleanupAction> actions,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(actions);

        return await Task.Run(() => ExecuteCleanupInternal(actions, cancellationToken), cancellationToken).ConfigureAwait(false);
    }

    private static List<CleanupAction> PreviewCleanupInternal(
        CleanupProfile profile,
        CancellationToken cancellationToken)
    {
        List<FileInfo> allFiles = [];
        ScanDirectory(new DirectoryInfo(profile.TargetDirectory), allFiles, cancellationToken);

        List<CleanupAction> actions = [];

        foreach (FileInfo file in allFiles)
        {
            cancellationToken.ThrowIfCancellationRequested();

            CleanupCategory? category = DetermineCategory(file, profile);
            if (category.HasValue && category.Value != CleanupCategory.Unknown)
            {
                // Verify safety check before compiling the preview action
                try
                {
                    ValidateSafePath(file.FullName);
                    actions.Add(new CleanupAction
                    {
                        FilePath = file.FullName,
                        SizeInBytes = file.Length,
                        Category = category.Value
                    });
                }
                catch (CriticalSecurityException)
                {
                    // Gracefully skip putting critical system files in the preview, even if rules matched
                }
            }
        }

        return actions;
    }

    private static CleanupResult ExecuteCleanupInternal(
        IReadOnlyList<CleanupAction> actions,
        CancellationToken cancellationToken)
    {
        int filesAttempted = 0;
        int filesDeleted = 0;
        long bytesReclaimed = 0;
        List<string> failedDeletions = [];

        foreach (CleanupAction action in actions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            filesAttempted++;

            try
            {
                // Double-guard: Enforce absolute path validation at the moment of execution
                ValidateSafePath(action.FilePath);

                if (File.Exists(action.FilePath))
                {
                    File.Delete(action.FilePath);
                    filesDeleted++;
                    bytesReclaimed += action.SizeInBytes;
                }
            }
            catch (CriticalSecurityException ex)
            {
                // Crucial security violation: Immediate termination of the entire execution to prevent damage
                throw new CriticalSecurityException($"Security Halt! Deletion execution aborted. {ex.Message}");
            }
            catch
            {
                // Gracefully handle locks, read-only permissions, or other FS access blocks
                failedDeletions.Add(action.FilePath);
            }
        }

        return new CleanupResult
        {
            FilesAttempted = filesAttempted,
            FilesDeleted = filesDeleted,
            BytesReclaimed = bytesReclaimed,
            FailedDeletions = failedDeletions
        };
    }

    private static void ScanDirectory(
        DirectoryInfo directory,
        List<FileInfo> files,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Safe skip on reparse points (symlinks)
        if (directory.Attributes.HasFlag(FileAttributes.ReparsePoint))
        {
            return;
        }

        try
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!file.Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    files.Add(file);
                }
            }

            foreach (DirectoryInfo subDir in directory.GetDirectories())
            {
                ScanDirectory(subDir, files, cancellationToken);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip unauthorized
        }
        catch (Exception)
        {
            // Skip general
        }
    }

    private static CleanupCategory? DetermineCategory(FileInfo file, CleanupProfile profile)
    {
        string extension = file.Extension;
        string? directoryPath = file.DirectoryName;

        if (string.IsNullOrEmpty(directoryPath))
        {
            return null;
        }

        // Get the immediate parent folder name (e.g. "LocalTemp" instead of the full path)
        string folderName = Path.GetFileName(directoryPath) ?? string.Empty;

        // 1. Temporary Files check
        if (profile.ActiveCategories.Contains(CleanupCategory.TemporaryFiles))
        {
            bool isTempFolder = TempIndicators.Any(indicator => folderName.Contains(indicator, StringComparison.OrdinalIgnoreCase));
            bool isTempExt = extension.Equals(".tmp", StringComparison.OrdinalIgnoreCase) || extension.Equals(".temp", StringComparison.OrdinalIgnoreCase);

            if (isTempFolder || isTempExt)
            {
                return CleanupCategory.TemporaryFiles;
            }
        }

        // 2. Recycle Bin check
        if (profile.ActiveCategories.Contains(CleanupCategory.RecycleBin))
        {
            if (folderName.Equals("$Recycle.Bin", StringComparison.OrdinalIgnoreCase) || directoryPath.Contains("$Recycle.Bin", StringComparison.OrdinalIgnoreCase))
            {
                return CleanupCategory.RecycleBin;
            }
        }

        // 3. Browser Cache check
        if (profile.ActiveCategories.Contains(CleanupCategory.BrowserCache))
        {
            bool isBrowser = directoryPath.Contains("Chrome", StringComparison.OrdinalIgnoreCase) ||
                             directoryPath.Contains("Edge", StringComparison.OrdinalIgnoreCase) ||
                             directoryPath.Contains("Firefox", StringComparison.OrdinalIgnoreCase);

            bool isCache = CacheIndicators.Any(indicator => folderName.Contains(indicator, StringComparison.OrdinalIgnoreCase));

            if (isBrowser && isCache)
            {
                return CleanupCategory.BrowserCache;
            }
        }

        // 4. Application Cache check
        if (profile.ActiveCategories.Contains(CleanupCategory.ApplicationCache))
        {
            bool isCacheFolder = CacheIndicators.Any(indicator => folderName.Contains(indicator, StringComparison.OrdinalIgnoreCase));
            bool isBakExt = extension.Equals(".bak", StringComparison.OrdinalIgnoreCase);

            if (isCacheFolder || isBakExt)
            {
                return CleanupCategory.ApplicationCache;
            }
        }

        // 5. System Logs check
        if (profile.ActiveCategories.Contains(CleanupCategory.SystemLogs))
        {
            bool isLogExt = LogExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            bool isLogFolder = folderName.Contains("logs", StringComparison.OrdinalIgnoreCase) || folderName.Contains("logging", StringComparison.OrdinalIgnoreCase);

            if (isLogExt || isLogFolder)
            {
                return CleanupCategory.SystemLogs;
            }
        }

        return CleanupCategory.Unknown;
    }

    private static void ValidateSafePath(string filePath)
    {
        string fullPath = Path.GetFullPath(filePath);

        // Retrieve real system drive, e.g., C:\
        string systemDrive = Path.GetPathRoot(Environment.SystemDirectory) ?? @"C:\";

        string[] blacklistedFolders =
        [
            Path.Combine(systemDrive, "Windows"),
            Path.Combine(systemDrive, "Program Files"),
            Path.Combine(systemDrive, "Program Files (x86)"),
            Path.Combine(systemDrive, "ProgramData")
        ];

        foreach (string folder in blacklistedFolders)
        {
            if (fullPath.StartsWith(folder, StringComparison.OrdinalIgnoreCase))
            {
                throw new CriticalSecurityException($"Security Violation: Deletion targeted a protected system directory: {folder}");
            }
        }

        // Ensure we don't delete files residing directly on the system drive root (e.g. C:\bootmgr)
        string? directoryPath = Path.GetDirectoryName(fullPath);
        if (string.Equals(directoryPath, systemDrive, StringComparison.OrdinalIgnoreCase))
        {
            throw new CriticalSecurityException($"Security Violation: Deletion targeted the root of the system drive: {systemDrive}");
        }
    }
}
