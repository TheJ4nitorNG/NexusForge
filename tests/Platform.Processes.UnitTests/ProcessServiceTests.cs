namespace Company.Platform.Processes.UnitTests;

using Company.Platform.Processes;
using FluentAssertions;

public class ProcessServiceTests
{
    [Fact]
    public async Task GetProcessesAsync_ShouldReturnProcesses()
    {
        // Arrange
        var service = new ProcessService();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        var processes = await service.GetProcessesAsync(cts.Token);

        // Assert
        processes.Should().NotBeNull();
        processes.Should().NotBeEmpty();

        // Ensure we can find our own process or a very common one like explorer/svchost
        var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        processes.Should().Contain(p => p.ProcessId == currentProcess.Id);
    }
}
