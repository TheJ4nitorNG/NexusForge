using FluentAssertions;

namespace Company.CMDPilot.Commands.UnitTests;

public class RiskResultTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        RiskResult result = new(RiskLevel.Moderate, "Moderate risk due to file modification");

        // Assert
        result.Level.Should().Be(RiskLevel.Moderate);
        result.Justification.Should().Be("Moderate risk due to file modification");
    }
}
