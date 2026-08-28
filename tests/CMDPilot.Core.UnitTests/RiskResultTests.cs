namespace Company.CMDPilot.Core.UnitTests;

using Company.CMDPilot.Core;
using FluentAssertions;

public class RiskResultTests
{
    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Arrange & Act
        var result = new RiskResult(RiskLevel.Moderate, "Moderate risk due to file modification");

        // Assert
        result.Level.Should().Be(RiskLevel.Moderate);
        result.Justification.Should().Be("Moderate risk due to file modification");
    }
}
