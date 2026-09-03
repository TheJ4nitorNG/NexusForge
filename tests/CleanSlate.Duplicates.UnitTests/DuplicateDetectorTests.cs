namespace Company.CleanSlate.Duplicates.UnitTests;

public class DuplicateDetectorTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly DuplicateDetector _detector = new();

    public DuplicateDetectorTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "CleanSlate_DupTests_" + Guid.NewGuid().ToString("N"));
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
    public async Task FindDuplicatesAsync_WithActualDuplicates_ReturnsDuplicateGroups()
    {
        // Arrange
        string file1Path = Path.Combine(_tempDirectory, "file1.txt");
        string file2Path = Path.Combine(_tempDirectory, "file2.txt");
        string file3Path = Path.Combine(_tempDirectory, "file3.txt"); // Not a duplicate (different size)
        string file4Path = Path.Combine(_tempDirectory, "file4.txt"); // Not a duplicate (same size as 1 and 2, but different header/content)

        // Write identical files (1 and 2)
        byte[] content1 = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        await File.WriteAllBytesAsync(file1Path, content1);
        await File.WriteAllBytesAsync(file2Path, content1);

        // Write file with different size (3)
        byte[] content3 = [1, 2, 3, 4, 5];
        await File.WriteAllBytesAsync(file3Path, content3);

        // Write file with same size but different content (4)
        byte[] content4 = [9, 9, 9, 9, 9, 9, 9, 9, 9, 9];
        await File.WriteAllBytesAsync(file4Path, content4);

        // Act
        IReadOnlyList<DuplicateGroup> groups = await _detector.FindDuplicatesAsync(_tempDirectory, CancellationToken.None);

        // Assert
        groups.Should().ContainSingle();
        DuplicateGroup group = groups[0];
        group.FileSize.Should().Be(content1.Length);
        group.FilePaths.Should().HaveCount(2);
        group.FilePaths.Should().Contain(file1Path);
        group.FilePaths.Should().Contain(file2Path);
    }

    [Fact]
    public async Task FindDuplicatesAsync_NoDuplicates_ReturnsEmpty()
    {
        // Arrange
        string file1Path = Path.Combine(_tempDirectory, "file1.txt");
        string file2Path = Path.Combine(_tempDirectory, "file2.txt");

        await File.WriteAllBytesAsync(file1Path, [1, 2, 3]);
        await File.WriteAllBytesAsync(file2Path, [4, 5, 6, 7]);

        // Act
        IReadOnlyList<DuplicateGroup> groups = await _detector.FindDuplicatesAsync(_tempDirectory, CancellationToken.None);

        // Assert
        groups.Should().BeEmpty();
    }

    [Fact]
    public async Task FindDuplicatesAsync_EmptyFiles_AreIgnored()
    {
        // Arrange
        string file1Path = Path.Combine(_tempDirectory, "empty1.txt");
        string file2Path = Path.Combine(_tempDirectory, "empty2.txt");

        await File.WriteAllBytesAsync(file1Path, []);
        await File.WriteAllBytesAsync(file2Path, []);

        // Act
        IReadOnlyList<DuplicateGroup> groups = await _detector.FindDuplicatesAsync(_tempDirectory, CancellationToken.None);

        // Assert
        groups.Should().BeEmpty(); // We ignore zero-byte files
    }

    [Fact]
    public async Task FindDuplicatesAsync_WithInvalidDirectory_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        string nonExistentPath = Path.Combine(_tempDirectory, "does_not_exist_folder");

        // Act & Assert
        await Assert.ThrowsAsync<DirectoryNotFoundException>(() =>
            _detector.FindDuplicatesAsync(nonExistentPath, CancellationToken.None));
    }
}
