namespace Company.CleanSlate.Scanner.UnitTests;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Company.CleanSlate.Scanner;
using FluentAssertions;

public class StorageScannerTests : IDisposable
{
    private readonly string _testDirectory;

    public StorageScannerTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), $"CleanSlateTest_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_testDirectory);

        // Create some dummy files
        File.WriteAllText(Path.Combine(_testDirectory, "test1.txt"), "Hello World");
        File.WriteAllText(Path.Combine(_testDirectory, "test2.txt"), "Another file with more content");

        // Create a subdirectory with a hidden file
        string subDir = Path.Combine(_testDirectory, "SubDir");
        Directory.CreateDirectory(subDir);
        string hiddenFilePath = Path.Combine(subDir, "hidden.txt");
        File.WriteAllText(hiddenFilePath, "Hidden content");
        File.SetAttributes(hiddenFilePath, FileAttributes.Hidden);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, recursive: true);
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task ScanAsync_ShouldTraverseDirectoriesAndCountFiles()
    {
        // Arrange
        var scanner = new StorageScanner();
        var options = new StorageScanOptions
        {
            TargetPath = _testDirectory,
            IncludeHiddenFiles = true,
            IncludeSystemFiles = true,
            IncludeProtectedPaths = true,
            MinimumFileSize = 0,
            CalculateHashes = false
        };

        // Act
        var result = await scanner.ScanAsync(options, null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalFiles.Should().Be(3);
        result.TotalDirectories.Should().Be(2); // Root + SubDir
        result.TotalBytes.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ScanAsync_ShouldRespectExcludeHiddenFilesOption()
    {
        // Arrange
        var scanner = new StorageScanner();
        var options = new StorageScanOptions
        {
            TargetPath = _testDirectory,
            IncludeHiddenFiles = false,
            IncludeSystemFiles = true,
            IncludeProtectedPaths = true,
            MinimumFileSize = 0,
            CalculateHashes = false
        };

        // Act
        var result = await scanner.ScanAsync(options, null, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.TotalFiles.Should().Be(2); // Only test1.txt and test2.txt, hidden.txt is excluded
        result.TotalDirectories.Should().Be(2);
    }
}
