using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics.UnitTests;

public class DiagnosticResultTests
{
    [Fact]
    public void DiagnosticResult_CanBeInitialized_WithFindings()
    {
        // Arrange
        DiagnosticFinding finding = new()
        {
            Id = "finding-1",
            Severity = DiagnosticStatus.Warning,
            Message = "Disk space is running low",
            Recommendation = "Free up 5GB"
        };

        // Act
        DiagnosticResult result = new()
        {
            CheckId = "disk-check",
            CheckName = "Disk Check",
            Status = DiagnosticStatus.Warning,
            Message = "Completed with warnings",
            Duration = TimeSpan.FromMilliseconds(50),
            Findings = [finding]
        };

        // Assert
        result.CheckId.Should().Be("disk-check");
        result.CheckName.Should().Be("Disk Check");
        result.Status.Should().Be(DiagnosticStatus.Warning);
        result.Findings.Should().ContainSingle();
        result.Findings[0].Message.Should().Be("Disk space is running low");
    }
}
