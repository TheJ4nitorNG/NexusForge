namespace Company.CMDPilot.Risk.UnitTests;

using Company.CMDPilot.Core;
using Company.CMDPilot.Risk;
using FluentAssertions;

public class RiskEngineTests
{
    [Fact]
    public void Evaluate_ShouldReturnSafe_ForKnownSafeCommands()
    {
        // Arrange
        var engine = new RiskEngine();

        var proposal = new CommandProposal
        {
            Id = "1",
            Shell = "powershell",
            CommandText = "Get-Process",
            Explanation = "Gets processes",
            RiskLevel = RiskLevel.Unknown,
            RequiredPrivilege = PrivilegeLevel.User,
            Effects = []
        };

        // Act
        var result = engine.Evaluate(proposal, isObfuscated: false);

        // Assert
        result.Level.Should().Be(RiskLevel.Safe);
    }

    [Fact]
    public void Evaluate_ShouldReturnHigh_ForUnknownCommands()
    {
        // Arrange
        var engine = new RiskEngine();

        var proposal = new CommandProposal
        {
            Id = "2",
            Shell = "powershell",
            CommandText = "Some-UnknownCommand",
            Explanation = "Unknown",
            RiskLevel = RiskLevel.Unknown,
            RequiredPrivilege = PrivilegeLevel.User,
            Effects = []
        };

        // Act
        var result = engine.Evaluate(proposal, isObfuscated: false);

        // Assert
        result.Level.Should().Be(RiskLevel.High);
    }

    [Fact]
    public void Evaluate_ShouldReturnCritical_WhenObfuscated()
    {
        // Arrange
        var engine = new RiskEngine();

        var proposal = new CommandProposal
        {
            Id = "3",
            Shell = "powershell",
            CommandText = "Get-Process", // Normally safe
            Explanation = "Gets processes",
            RiskLevel = RiskLevel.Unknown,
            RequiredPrivilege = PrivilegeLevel.User,
            Effects = []
        };

        // Act
        var result = engine.Evaluate(proposal, isObfuscated: true);

        // Assert
        result.Level.Should().Be(RiskLevel.Critical);
        result.Justification.Should().Contain("obfuscation");
    }

    [Fact]
    public void Evaluate_ShouldReturnModerate_ForModifyingEffects()
    {
        // Arrange
        var engine = new RiskEngine();

        var proposal = new CommandProposal
        {
            Id = "4",
            Shell = "powershell",
            CommandText = "Restart-Service Spooler",
            Explanation = "Restarts spooler",
            RiskLevel = RiskLevel.Unknown,
            RequiredPrivilege = PrivilegeLevel.Administrator,
            Effects = [new CommandEffect(EffectType.RestartService, "Restarts Spooler", EffectSeverity.Moderate)]
        };

        // Act
        var result = engine.Evaluate(proposal, isObfuscated: false);

        // Assert
        // The engine should elevate risk if it requires Administrator or has modifying effects
        result.Level.Should().Be(RiskLevel.Moderate);
    }
}
