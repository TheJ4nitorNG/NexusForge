namespace Company.SysMedic.Diagnostics.Windows.UnitTests;

public class LogicalDiskSpaceCheckTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldEvaluateDiskSpaceWithoutCrashing()
    {
        // Arrange
        var check = new LogicalDiskSpaceCheck();

        var snapshot = Substitute.For<ISystemSnapshot>();
        var context = new DiagnosticContext
        {
            ScanId = "scan-1",
            StartedAt = DateTimeOffset.UtcNow,
            CancellationToken = CancellationToken.None,
            Snapshot = snapshot
        };

        // Act
        var result = await check.ExecuteAsync(context, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CheckId.Should().Be("windows.storage.freespace");

        // Since we are running on a real machine, it should either pass or have findings
        if (result.Status == DiagnosticStatus.Passed)
        {
            result.Findings.Should().BeEmpty();
        }
        else
        {
            result.Findings.Should().NotBeEmpty();
            result.Severity.Should().BeOneOf(DiagnosticSeverity.Moderate, DiagnosticSeverity.Critical);
        }
    }
}
