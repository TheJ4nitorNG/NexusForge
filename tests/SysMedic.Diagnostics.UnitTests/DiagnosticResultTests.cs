namespace Company.SysMedic.Diagnostics.UnitTests;

using Company.SysMedic.Diagnostics;

public class DiagnosticResultTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var findings = new List<DiagnosticFinding>
        {
            new("TEST_01", DiagnosticSeverity.Information, "Test Finding", "A finding", new Dictionary<string, object?>())
        };

        // Act
        var result = new DiagnosticResult
        {
            CheckId = "test.check",
            Status = DiagnosticStatus.Passed,
            Severity = DiagnosticSeverity.Information,
            Summary = "Test passed",
            Findings = findings
        };

        // Assert
        result.CheckId.Should().Be("test.check");
        result.Status.Should().Be(DiagnosticStatus.Passed);
        result.Severity.Should().Be(DiagnosticSeverity.Information);
        result.Summary.Should().Be("Test passed");
        result.Findings.Should().BeEquivalentTo(findings);
    }
}
