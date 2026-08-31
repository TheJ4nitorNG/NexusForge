namespace Company.CleanSlate.Categorization.UnitTests;

using Company.CleanSlate.Categorization;
using FluentAssertions;

public class CategorizationEngineTests
{
    [Fact]
    public void Classify_ShouldReturnTemporary_ForTempExtension()
    {
        // Arrange
        var rules = new IStorageClassificationRule[] { new ExtensionRule(), new PathRule() };
        var engine = new CategorizationEngine(rules);
        var file = new FileMetadata(@"C:\Data\somefile.tmp", ".tmp", 1024);

        // Act
        var result = engine.Classify(file);

        // Assert
        result.Category.Should().Be(StorageCategory.Temporary);
        result.Confidence.Should().Be(80);
    }

    [Fact]
    public void Classify_ShouldReturnWindows_ForWindowsPath()
    {
        // Arrange
        var rules = new IStorageClassificationRule[] { new ExtensionRule(), new PathRule() };
        var engine = new CategorizationEngine(rules);
        var file = new FileMetadata(@"C:\Windows\System32\kernel32.dll", ".dll", 102400);

        // Act
        var result = engine.Classify(file);

        // Assert
        result.Category.Should().Be(StorageCategory.Windows);
        result.Confidence.Should().Be(100);
    }

    [Fact]
    public void Classify_ShouldReturnTemporary_ForWindowsTempPath_OverridingWindowsCategory()
    {
        // Arrange
        var rules = new IStorageClassificationRule[] { new ExtensionRule(), new PathRule() };
        var engine = new CategorizationEngine(rules);

        // This file is in Windows (Confidence 100 base) but Temp specifically gives it Temporary (Confidence 90).
        // Wait, PathRule returns the FIRST match. Let's ensure PathRule logic correctly prefers Temp.
        var file = new FileMetadata(@"C:\Windows\Temp\garbage.log", ".log", 1024);

        // Act
        var result = engine.Classify(file);

        // Assert
        result.Category.Should().Be(StorageCategory.Temporary);
        result.Confidence.Should().Be(90);
    }

    [Fact]
    public void Classify_ShouldReturnInstallers_ForMsi()
    {
        // Arrange
        var rules = new IStorageClassificationRule[] { new ExtensionRule(), new PathRule() };
        var engine = new CategorizationEngine(rules);
        var file = new FileMetadata(@"C:\Downloads\installer.msi", ".msi", 1024);

        // Act
        var result = engine.Classify(file);

        // Assert
        result.Category.Should().Be(StorageCategory.Installers);
        result.Confidence.Should().Be(90);
    }
}
