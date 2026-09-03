namespace Company.CleanSlate.Cleanup;

/// <summary>
/// Specifies categories of data that can be targeted for cleanup.
/// </summary>
public enum CleanupCategory
{
    /// <summary>Category is unknown.</summary>
    Unknown = 0,
    /// <summary>Temporary operating system files.</summary>
    TemporaryFiles = 1,
    /// <summary>Recycle Bin items.</summary>
    RecycleBin = 2,
    /// <summary>Application-specific caches.</summary>
    ApplicationCache = 3,
    /// <summary>Web browser caches and history databases.</summary>
    BrowserCache = 4,
    /// <summary>System logs and diagnostic dumps.</summary>
    SystemLogs = 5
}
