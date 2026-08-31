namespace Company.SysMedic.Diagnostics.UnitTests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Company.SysMedic.Diagnostics;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

public class DiagnosticCoordinatorTests
{
    [Fact]
    public async Task RunScanAsync_ShouldExecuteAllChecksAndCalculateHealthScore()
    {
        // Arrange
        var check1 = Substitute.For<IDiagnosticCheck>();
        check1.Id.Returns("check1");
        check1.ExecuteAsync(Arg.Any<DiagnosticContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new DiagnosticResult
            {
                CheckId = "check1",
                Status = DiagnosticStatus.Passed,
                Severity = DiagnosticSeverity.Information,
                Summary = "Check 1 Passed"
            }));

        var check2 = Substitute.For<IDiagnosticCheck>();
        check2.Id.Returns("check2");
        check2.ExecuteAsync(Arg.Any<DiagnosticContext>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new DiagnosticResult
            {
                CheckId = "check2",
                Status = DiagnosticStatus.Failed,
                Severity = DiagnosticSeverity.High,
                Summary = "Check 2 Failed"
            }));

        var snapshot = Substitute.For<ISystemSnapshot>();
        var context = new DiagnosticContext
        {
            ScanId = "scan-123",
            StartedAt = DateTimeOffset.UtcNow,
            CancellationToken = CancellationToken.None,
            Snapshot = snapshot
        };

        var coordinator = new DiagnosticCoordinator([check1, check2]);

        // Act
        var report = await coordinator.RunScanAsync(context, CancellationToken.None);

        // Assert
        report.Should().NotBeNull();
        report.ScanId.Should().Be("scan-123");
        report.Results.Should().HaveCount(2);

        // 100 - 5 (high severity failure) = 95
        report.OverallHealthScore.Should().Be(95);
    }

    [Fact]
    public async Task RunScanAsync_ShouldHandleCheckExceptionsGracefully()
    {
        // Arrange
        var check1 = Substitute.For<IDiagnosticCheck>();
        check1.Id.Returns("check1");
        check1.ExecuteAsync(Arg.Any<DiagnosticContext>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Something crashed"));

        var snapshot = Substitute.For<ISystemSnapshot>();
        var context = new DiagnosticContext
        {
            ScanId = "scan-456",
            StartedAt = DateTimeOffset.UtcNow,
            CancellationToken = CancellationToken.None,
            Snapshot = snapshot
        };

        var coordinator = new DiagnosticCoordinator([check1]);

        // Act
        var report = await coordinator.RunScanAsync(context, CancellationToken.None);

        // Assert
        report.Should().NotBeNull();
        report.Results.Should().HaveCount(1);
        var result = report.Results[0];
        result.CheckId.Should().Be("check1");
        result.Status.Should().Be(DiagnosticStatus.Error);
        result.Severity.Should().Be(DiagnosticSeverity.Critical);
        result.Summary.Should().Contain("unexpected error");
        result.Details.Should().Contain("Something crashed");

        // 100 - 10 (critical error) = 90
        report.OverallHealthScore.Should().Be(90);
    }
}
