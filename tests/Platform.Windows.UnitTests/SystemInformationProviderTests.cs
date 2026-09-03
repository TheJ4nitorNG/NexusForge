namespace Company.Platform.Windows.UnitTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SystemInformationProviderTests
{
    [Xunit.Fact]
    public async System.Threading.Tasks.Task GetSystemInformationAsync_ReturnsValidSnapshot()
    {
        using SystemInformationProvider provider = new();
        Company.Platform.Abstractions.SystemInformation info = await provider.GetSystemInformationAsync(System.Threading.CancellationToken.None);

        Xunit.Assert.NotNull(info);
        Xunit.Assert.False(string.IsNullOrEmpty(info.OsVersion));
        Xunit.Assert.False(string.IsNullOrEmpty(info.Architecture));
        Xunit.Assert.False(string.IsNullOrEmpty(info.MachineName));
        Xunit.Assert.False(string.IsNullOrEmpty(info.CpuName));
        Xunit.Assert.True(info.SystemUptime > System.TimeSpan.Zero);
    }

    [Xunit.Fact]
    public async System.Threading.Tasks.Task GetSystemInformationAsync_WhenCalledMultipleTimes_UsesCacheAndUpdatesUptime()
    {
        using SystemInformationProvider provider = new();
        Company.Platform.Abstractions.SystemInformation firstCall = await provider.GetSystemInformationAsync(System.Threading.CancellationToken.None);

        await System.Threading.Tasks.Task.Delay(50);

        Company.Platform.Abstractions.SystemInformation secondCall = await provider.GetSystemInformationAsync(System.Threading.CancellationToken.None);

        Xunit.Assert.Equal(firstCall.OsVersion, secondCall.OsVersion);
        Xunit.Assert.Equal(firstCall.Architecture, secondCall.Architecture);
        Xunit.Assert.Equal(firstCall.MachineName, secondCall.MachineName);
        Xunit.Assert.Equal(firstCall.CpuName, secondCall.CpuName);
        Xunit.Assert.Equal(firstCall.TotalPhysicalMemoryBytes, secondCall.TotalPhysicalMemoryBytes);
        Xunit.Assert.True(secondCall.SystemUptime > firstCall.SystemUptime);
    }
}
