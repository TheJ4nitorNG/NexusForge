using Company.Platform.Abstractions.Diagnostics;

namespace Company.SysMedic.Diagnostics.Windows.UnitTests;

public class LogicalDiskSpaceCheckTests
{
    [Xunit.Fact]
    public async System.Threading.Tasks.Task ExecuteAsync_ReturnsValidDiagnosticResult()
    {
        LogicalDiskSpaceCheck check = new();
        DiagnosticContext context = new();

        DiagnosticResult result = await check.ExecuteAsync(context);

        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("windows.storage.freespace", result.CheckId);
        Xunit.Assert.NotEqual(DiagnosticStatus.Skipped, result.Status);
        Xunit.Assert.NotEqual(DiagnosticStatus.Unknown, result.Status);
    }
}
