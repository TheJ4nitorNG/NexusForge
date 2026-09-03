using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics.UnitTests;

public class DiagnosticCoordinatorTests
{
    [Fact]
    public async Task RunScanAsync_WithPassingChecks_ReturnsHealthyScore()
    {
        // Arrange
        IDiagnosticCheck check1 = Substitute.For<IDiagnosticCheck>();
        check1.Id.Returns("check-1");
        check1.Name.Returns("Check 1");

        check1.ExecuteAsync(Arg.Any<DiagnosticContext>())
            .Returns(Task.FromResult(new DiagnosticResult
            {
                CheckId = "check-1",
                CheckName = "Check 1",
                Status = DiagnosticStatus.Healthy,
                Message = "All good"
            }));

        IDiagnosticCheck check2 = Substitute.For<IDiagnosticCheck>();
        check2.Id.Returns("check-2");
        check2.Name.Returns("Check 2");

        check2.ExecuteAsync(Arg.Any<DiagnosticContext>())
            .Returns(Task.FromResult(new DiagnosticResult
            {
                CheckId = "check-2",
                CheckName = "Check 2",
                Status = DiagnosticStatus.Healthy,
                Message = "All good here too"
            }));

        DiagnosticCoordinator coordinator = new([check1, check2]);
        DiagnosticContext context = new();

        // Act
        ScanReport report = await coordinator.RunScanAsync("scan-123", context);

        // Assert
        report.Should().NotBeNull();
        report.ScanId.Should().Be("scan-123");
        report.OverallHealthScore.Should().Be(100);
        report.Results.Should().HaveCount(2);
        report.Results.All(r => r.Status == DiagnosticStatus.Healthy).Should().BeTrue();
    }

    [Fact]
    public async Task RunScanAsync_WhenCheckThrowsException_CatchesAndReturnsErrorResult()
    {
        // Arrange
        IDiagnosticCheck check = Substitute.For<IDiagnosticCheck>();
        check.Id.Returns("faulty-check");
        check.Name.Returns("Faulty Check");

        check.ExecuteAsync(Arg.Any<DiagnosticContext>())
            .Returns(Task.FromException<DiagnosticResult>(new InvalidOperationException("Simulated failure")));

        DiagnosticCoordinator coordinator = new([check]);
        DiagnosticContext context = new();

        // Act
        ScanReport report = await coordinator.RunScanAsync("scan-456", context);

        // Assert
        report.Should().NotBeNull();
        report.OverallHealthScore.Should().Be(90);
        report.Results.Should().ContainSingle();

        DiagnosticResult result = report.Results[0];
        result.CheckId.Should().Be("faulty-check");
        result.Status.Should().Be(DiagnosticStatus.Error);
        result.Message.Should().Contain("Simulated failure");
    }
}
