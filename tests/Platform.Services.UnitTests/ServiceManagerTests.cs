namespace Company.Platform.Services.UnitTests;

using Company.Platform.Services;
using FluentAssertions;

public class ServiceManagerTests
{
    [Fact]
    public async Task GetServicesAsync_ShouldReturnServices()
    {
        // Arrange
        var manager = new ServiceManager();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        var services = await manager.GetServicesAsync(cts.Token);

        // Assert
        services.Should().NotBeNull();
        services.Should().NotBeEmpty();

        // Verify some common Windows services exist (e.g. LanmanWorkstation or Spooler)
        services.Should().Contain(s =>
            s.ServiceName.Equals("LanmanWorkstation", StringComparison.OrdinalIgnoreCase) ||
            s.ServiceName.Equals("Spooler", StringComparison.OrdinalIgnoreCase) ||
            s.ServiceName.Equals("EventLog", StringComparison.OrdinalIgnoreCase));
    }
}
