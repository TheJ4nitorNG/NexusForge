using Company.Platform.Abstractions;

namespace Company.SysMedic.Diagnostics.Windows.UnitTests;

public class CriticalServicesCheckTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldDetectMissingOrStoppedServices()
    {
        // Arrange
        var serviceManager = Substitute.For<IServiceManager>();
        serviceManager.GetServicesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ServiceInfo>>([
                new ServiceInfo("Winmgmt", "WMI", "Running", "Auto"),
                new ServiceInfo("EventLog", "Event Log", "Stopped", "Manual")
                // RpcSs is intentionally omitted
            ]));

        var check = new CriticalServicesCheck(serviceManager);

        var snapshot = Substitute.For<ISystemSnapshot>();
        var context = new DiagnosticContext
        {
            ScanId = "scan-2",
            StartedAt = DateTimeOffset.UtcNow,
            CancellationToken = CancellationToken.None,
            Snapshot = snapshot
        };

        // Act
        var result = await check.ExecuteAsync(context, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(DiagnosticStatus.Failed);
        result.Severity.Should().Be(DiagnosticSeverity.Critical); // Because RpcSs is missing
        result.Findings.Should().HaveCount(2);

        result.Findings.Should().ContainSingle(f => f.Code == "SERVICE_STOPPED" && f.Title.Contains("EventLog"));
        result.Findings.Should().ContainSingle(f => f.Code == "SERVICE_MISSING" && f.Title.Contains("RpcSs"));
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPassWhenAllAreRunning()
    {
        // Arrange
        var serviceManager = Substitute.For<IServiceManager>();
        serviceManager.GetServicesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<ServiceInfo>>([
                new ServiceInfo("Winmgmt", "WMI", "Running", "Auto"),
                new ServiceInfo("EventLog", "Event Log", "Running", "Auto"),
                new ServiceInfo("RpcSs", "RPC", "Running", "Auto")
            ]));

        var check = new CriticalServicesCheck(serviceManager);

        var snapshot = Substitute.For<ISystemSnapshot>();
        var context = new DiagnosticContext
        {
            ScanId = "scan-3",
            StartedAt = DateTimeOffset.UtcNow,
            CancellationToken = CancellationToken.None,
            Snapshot = snapshot
        };

        // Act
        var result = await check.ExecuteAsync(context, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(DiagnosticStatus.Passed);
        result.Findings.Should().BeEmpty();
    }
}
