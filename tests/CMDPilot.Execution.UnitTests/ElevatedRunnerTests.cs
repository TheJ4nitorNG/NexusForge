using FluentAssertions;
using Company.Platform.Abstractions;
using Company.CMDPilot.Risk;

namespace Company.CMDPilot.Execution.UnitTests;

public class ElevatedRunnerTests
{
    private readonly RiskEngine _riskEngine = new();
    private readonly ElevatedRunner _runner;

    public ElevatedRunnerTests()
    {
        _runner = new ElevatedRunner(_riskEngine);
    }

    [Fact]
    public async Task ExecuteAsync_SafeCommand_Succeeds()
    {
        // Arrange
        CommandRequest request = new(
            "Get-Process",
            ["-Name", "idle"],
            ExecutionPolicy.ReadOnly,
            RequiresElevation: false);

        // Act
        CommandExecutionResult result = await _runner.ExecuteAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ExitCode.Should().Be(0); // idle process always exists on Windows
        result.StandardOutput.Should().Contain("Idle");
        result.StandardError.Should().BeEmpty();
        result.ExecutionDuration.Should().BeGreaterThan(TimeSpan.Zero);
    }

    [Fact]
    public async Task ExecuteAsync_UnverifiedCommand_ReturnsBlockedResult()
    {
        // Arrange: An unknown command is evaluated as High risk by default
        CommandRequest request = new(
            "Invoke-MaliciousCmdlet",
            [],
            ExecutionPolicy.LowRisk,
            RequiresElevation: false);

        // Act
        CommandExecutionResult result = await _runner.ExecuteAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ExitCode.Should().Be(-1);
        result.StandardOutput.Should().BeEmpty();
        result.StandardError.Should().Contain("blocked by CMDPilot Safety Policy");
        result.ExecutionDuration.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public async Task ExecuteAsync_UnverifiedCommand_WithDestructivePolicy_BypassesBlock()
    {
        // Arrange: Override high risk by using ExecutionPolicy.Destructive
        CommandRequest request = new(
            "Invoke-NonexistentCmdlet",
            [],
            ExecutionPolicy.Destructive,
            RequiresElevation: false);

        // Act
        CommandExecutionResult result = await _runner.ExecuteAsync(request, CancellationToken.None);

        // Assert: It should bypass the safety block and attempt execution, returning exit code 1 due to PowerShell throw
        result.Should().NotBeNull();
        result.ExitCode.Should().Be(1);
        result.StandardError.Should().Contain("is not recognized as the name of a cmdlet"); // Proves it tried to run the cmdlet
    }
}
