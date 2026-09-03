using Company.Platform.Abstractions;
using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics.Windows.UnitTests;

public class CriticalServicesCheckTests
{
    [Xunit.Fact]
    public async System.Threading.Tasks.Task ExecuteAsync_WhenAllServicesRunning_ReturnsHealthy()
    {
        IServiceManager serviceManager = NSubstitute.Substitute.For<IServiceManager>();
        serviceManager.GetServicesAsync(NSubstitute.Arg.Any<System.Threading.CancellationToken>()).Returns(System.Threading.Tasks.Task.FromResult<System.Collections.Generic.IReadOnlyList<ServiceInfo>>([
            new ServiceInfo("Winmgmt", "Windows Management Instrumentation", "Running", "Auto"),
            new ServiceInfo("EventLog", "Windows Event Log", "Running", "Auto"),
            new ServiceInfo("RpcSs", "Remote Procedure Call (RPC)", "Running", "Auto")
        ]));

        CriticalServicesCheck check = new(serviceManager);
        DiagnosticContext context = new();

        DiagnosticResult result = await check.ExecuteAsync(context);

        Xunit.Assert.Equal(DiagnosticStatus.Healthy, result.Status);
        Xunit.Assert.Empty(result.Findings);
    }

    [Xunit.Fact]
    public async System.Threading.Tasks.Task ExecuteAsync_WhenServiceIsStopped_ReturnsError()
    {
        IServiceManager serviceManager = NSubstitute.Substitute.For<IServiceManager>();
        serviceManager.GetServicesAsync(NSubstitute.Arg.Any<System.Threading.CancellationToken>()).Returns(System.Threading.Tasks.Task.FromResult<System.Collections.Generic.IReadOnlyList<ServiceInfo>>([
            new ServiceInfo("Winmgmt", "Windows Management Instrumentation", "Stopped", "Auto"),
            new ServiceInfo("EventLog", "Windows Event Log", "Running", "Auto"),
            new ServiceInfo("RpcSs", "Remote Procedure Call (RPC)", "Running", "Auto")
        ]));

        CriticalServicesCheck check = new(serviceManager);
        DiagnosticContext context = new();

        DiagnosticResult result = await check.ExecuteAsync(context);

        Xunit.Assert.Equal(DiagnosticStatus.Error, result.Status);
        Xunit.Assert.Single(result.Findings);
        Xunit.Assert.Equal("SERVICE_STOPPED", result.Findings[0].Id);
        Xunit.Assert.Contains("Winmgmt", result.Findings[0].Message);
    }

    [Xunit.Fact]
    public async System.Threading.Tasks.Task ExecuteAsync_WhenServiceIsMissing_ReturnsCritical()
    {
        IServiceManager serviceManager = NSubstitute.Substitute.For<IServiceManager>();
        serviceManager.GetServicesAsync(NSubstitute.Arg.Any<System.Threading.CancellationToken>()).Returns(System.Threading.Tasks.Task.FromResult<System.Collections.Generic.IReadOnlyList<ServiceInfo>>([
            new ServiceInfo("EventLog", "Windows Event Log", "Running", "Auto"),
            new ServiceInfo("RpcSs", "Remote Procedure Call (RPC)", "Running", "Auto")
        ]));

        CriticalServicesCheck check = new(serviceManager);
        DiagnosticContext context = new();

        DiagnosticResult result = await check.ExecuteAsync(context);

        Xunit.Assert.Equal(DiagnosticStatus.Critical, result.Status);
        Xunit.Assert.Single(result.Findings);
        Xunit.Assert.Equal("SERVICE_MISSING", result.Findings[0].Id);
        Xunit.Assert.Contains("Winmgmt", result.Findings[0].Message);
    }
}
