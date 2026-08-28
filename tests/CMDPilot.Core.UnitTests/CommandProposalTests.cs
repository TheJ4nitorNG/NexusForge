namespace Company.CMDPilot.Core.UnitTests;

using Company.CMDPilot.Core;
using FluentAssertions;

public class CommandEffectTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        var effect = new CommandEffect(EffectType.DeleteFile, "Deletes C:\\temp", EffectSeverity.High);

        // Assert
        effect.Type.Should().Be(EffectType.DeleteFile);
        effect.Description.Should().Be("Deletes C:\\temp");
        effect.Severity.Should().Be(EffectSeverity.High);
    }
}

public class CommandProposalTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        var effects = new[] { new CommandEffect(EffectType.ReadFile, "Reads file", EffectSeverity.Low) };

        // Act
        var proposal = new CommandProposal
        {
            Id = "test-id",
            Shell = "powershell",
            CommandText = "Get-Content test.txt",
            Explanation = "Reads the content of test.txt",
            RiskLevel = RiskLevel.Low,
            RequiredPrivilege = PrivilegeLevel.User,
            Effects = effects
        };

        // Assert
        proposal.Id.Should().Be("test-id");
        proposal.Shell.Should().Be("powershell");
        proposal.CommandText.Should().Be("Get-Content test.txt");
        proposal.Explanation.Should().Be("Reads the content of test.txt");
        proposal.RiskLevel.Should().Be(RiskLevel.Low);
        proposal.RequiredPrivilege.Should().Be(PrivilegeLevel.User);
        proposal.Effects.Should().BeEquivalentTo(effects);
    }
}
