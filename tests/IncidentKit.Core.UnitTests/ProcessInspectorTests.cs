using System.Runtime.Versioning;

namespace Company.IncidentKit.Core.UnitTests;

[SupportedOSPlatform("windows")]
public class ProcessInspectorTests
{
    [Fact]
    public void CaptureActiveProcesses_ReturnsListContainingCurrentProcess()
    {
        // Act
        IReadOnlyList<ProcessSnapshot> snapshots = ProcessInspector.CaptureActiveProcesses();

        // Assert
        snapshots.Should().NotBeEmpty();

        // Current executing test process should be in the snapshot list
        int currentPid = Environment.ProcessId;
        ProcessSnapshot? currentSnapshot = snapshots.FirstOrDefault(s => s.ProcessId == currentPid);

        currentSnapshot.Should().NotBeNull();
        currentSnapshot!.ProcessName.Should().NotBeNullOrWhiteSpace();
        currentSnapshot.WorkingSetMemory.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CaptureActiveProcesses_CurrentProcessContainsExpectedModules()
    {
        // Act
        IReadOnlyList<ProcessSnapshot> snapshots = ProcessInspector.CaptureActiveProcesses();

        // Assert
        int currentPid = Environment.ProcessId;
        ProcessSnapshot? currentSnapshot = snapshots.FirstOrDefault(s => s.ProcessId == currentPid);

        currentSnapshot.Should().NotBeNull();

        // The current .NET process should have some loaded modules/DLLs (e.g. System.Private.CoreLib.dll or host dlls)
        currentSnapshot!.Modules.Should().NotBeEmpty();
        bool hasCoreLib = currentSnapshot.Modules.Any(m => m.ModuleName.Contains("System.Private.CoreLib", StringComparison.OrdinalIgnoreCase));
        hasCoreLib.Should().BeTrue();
    }
}
