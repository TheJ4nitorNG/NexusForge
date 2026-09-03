using FluentAssertions;

namespace Company.CMDPilot.Commands.UnitTests;

public class CommandEffectTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        CommandEffect effect = new()
        {
            Description = "Deletes C:\\temp",
            Risk = RiskLevel.High,
            IsDestructive = true
        };

        // Assert
        effect.Description.Should().Be("Deletes C:\\temp");
        effect.Risk.Should().Be(RiskLevel.High);
        effect.IsDestructive.Should().BeTrue();
    }
}

public class CommandProposalTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange
        CommandEffect[] effects =
        [
            new CommandEffect
            {
                Description = "Reads file",
                Risk = RiskLevel.Low,
                IsDestructive = false
            }
        ];

        // Act
        CommandProposal proposal = new()
        {
            CommandText = "Get-Content test.txt",
            Purpose = "Reads the content of test.txt",
            RequiredPrivilege = PrivilegeLevel.Standard,
            Effects = effects
        };

        // Assert
        proposal.CommandText.Should().Be("Get-Content test.txt");
        proposal.Purpose.Should().Be("Reads the content of test.txt");
        proposal.RequiredPrivilege.Should().Be(PrivilegeLevel.Standard);
        proposal.Effects.Should().BeEquivalentTo(effects);
    }
}
