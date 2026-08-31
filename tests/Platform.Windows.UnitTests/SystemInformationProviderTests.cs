namespace Company.Platform.Windows.UnitTests;

using Company.Platform.Windows;
using FluentAssertions;

public class SystemInformationProviderTests
{
    [Fact]
    public async Task GetSystemInformationAsync_ShouldReturnRealSystemInfo()
    {
        // Arrange
        var provider = new SystemInformationProvider();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Act
        var info = await provider.GetSystemInformationAsync(cts.Token);

        // Assert
        info.Should().NotBeNull();
        info.OsVersion.Should().NotBeNullOrWhiteSpace();
        info.Architecture.Should().NotBeNullOrWhiteSpace();
        info.MachineName.Should().Be(Environment.MachineName);
    }
}
