namespace Company.CleanSlate.Scanner.UnitTests;

using Company.CleanSlate.Scanner;
using FluentAssertions;

public class StorageScanOptionsTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        var options = new StorageScanOptions
        {
            IncludeSystemFiles = true,
            IncludeHiddenFiles = true,
            IncludeProtectedPaths = false,
            MinimumFileSize = 1024,
            CalculateHashes = true
        };

        // Assert
        options.IncludeSystemFiles.Should().BeTrue();
        options.IncludeHiddenFiles.Should().BeTrue();
        options.IncludeProtectedPaths.Should().BeFalse();
        options.MinimumFileSize.Should().Be(1024);
        options.CalculateHashes.Should().BeTrue();
    }
}
