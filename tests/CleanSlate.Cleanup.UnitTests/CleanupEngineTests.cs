namespace Company.CleanSlate.Cleanup.UnitTests;

public class CleanupEngineTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly CleanupEngine _engine = new();

    public CleanupEngineTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "CleanSlate_CleanupTests_" + Guid.NewGuid().ToString("N"));
        _ = Directory.CreateDirectory(_tempDirectory);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }
        catch
        {
            // Ignore clean-up issues
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task PreviewAndExecute_WithValidFiles_SucceedsSafely()
    {
        // Arrange: Create a temporary directory structured as a "temp" cache folder
        string tempCacheDir = Path.Combine(_tempDirectory, "LocalTemp");
        _ = Directory.CreateDirectory(tempCacheDir);

        string tempFile1 = Path.Combine(tempCacheDir, "cache1.tmp");
        string tempFile2 = Path.Combine(tempCacheDir, "log1.log");
        string safeFile = Path.Combine(_tempDirectory, "important_doc.docx"); // Should not be cleaned

        await File.WriteAllTextAsync(tempFile1, "some temporary data");
        await File.WriteAllTextAsync(tempFile2, "log message");
        await File.WriteAllTextAsync(safeFile, "user document");

        CleanupProfile profile = new()
        {
            TargetDirectory = _tempDirectory,
            ActiveCategories = new HashSet<CleanupCategory>
            {
                CleanupCategory.TemporaryFiles,
                CleanupCategory.SystemLogs
            }
        };

        // Act: Stage 1 - Preview
        IReadOnlyList<CleanupAction> previewActions = await _engine.PreviewCleanupAsync(profile, CancellationToken.None);

        // Assert Stage 1
        previewActions.Should().HaveCount(2);
        previewActions.Any(a => a.FilePath == tempFile1).Should().BeTrue();
        previewActions.Any(a => a.FilePath == tempFile2).Should().BeTrue();
        previewActions.Any(a => a.FilePath == safeFile).Should().BeFalse();

        // Act: Stage 2 - Execute Deletion
        CleanupResult result = await _engine.ExecuteCleanupAsync(previewActions, CancellationToken.None);

        // Assert Stage 2
        result.FilesAttempted.Should().Be(2);
        result.FilesDeleted.Should().Be(2);
        result.BytesReclaimed.Should().BeGreaterThan(0);
        result.FailedDeletions.Should().BeEmpty();

        File.Exists(tempFile1).Should().BeFalse();
        File.Exists(tempFile2).Should().BeFalse();
        File.Exists(safeFile).Should().BeTrue(); // Confirmed safe file remains untouched
    }

    [Fact]
    public async Task ExecuteCleanupAsync_WhenTargetingSystemWindows_ThrowsCriticalSecurityException()
    {
        // Arrange: Simulate a malicious or buggy action targeting the system root folder
        string systemDirectory = Environment.SystemDirectory; // typically C:\Windows\System32
        string fakeTarget = Path.Combine(systemDirectory, "critical_file.sys");

        CleanupAction violationAction = new()
        {
            FilePath = fakeTarget,
            SizeInBytes = 1024,
            Category = CleanupCategory.TemporaryFiles
        };

        // Act & Assert: The engine must immediately halt with a CriticalSecurityException
        await Assert.ThrowsAsync<CriticalSecurityException>(() =>
            _engine.ExecuteCleanupAsync([violationAction], CancellationToken.None));
    }

    [Fact]
    public async Task ExecuteCleanupAsync_WhenTargetingSystemRoot_ThrowsCriticalSecurityException()
    {
        // Arrange: Target the exact root directory (e.g. C:\bootmgr)
        string systemDrive = Path.GetPathRoot(Environment.SystemDirectory) ?? @"C:\";
        string fakeTarget = Path.Combine(systemDrive, "bootmgr");

        CleanupAction violationAction = new()
        {
            FilePath = fakeTarget,
            SizeInBytes = 2048,
            Category = CleanupCategory.SystemLogs
        };

        // Act & Assert
        await Assert.ThrowsAsync<CriticalSecurityException>(() =>
            _engine.ExecuteCleanupAsync([violationAction], CancellationToken.None));
    }
}
