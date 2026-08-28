namespace Company.CleanSlate.Scanner;

/// <summary>
/// Represents the progress of an ongoing storage scan.
/// </summary>
/// <param name="FilesScanned">The number of files scanned so far.</param>
/// <param name="DirectoriesScanned">The number of directories scanned so far.</param>
/// <param name="TotalBytesFound">The total size of files found in bytes.</param>
/// <param name="CurrentDirectory">The path of the directory currently being scanned.</param>
public sealed record ScanProgress(
    long FilesScanned,
    long DirectoriesScanned,
    long TotalBytesFound,
    string CurrentDirectory);
