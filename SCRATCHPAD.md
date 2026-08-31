# SCRATCHPAD

## Track 006: Core Execution and Security Boundary

**Plan:**
1. Scaffold projects:
   - `src/Platform/Platform.Windows`
   - `src/Platform/Platform.IPC`
   - `tests/Platform.Windows.UnitTests`
   - `tests/Platform.IPC.UnitTests`
2. Add to `Company.Platform.sln`.
3. Fix test `.csproj` with `replace` tool to add `<NoWarn>` and `<GenerateDocumentationFile>true</GenerateDocumentationFile>`.
4. Delete default `Class1.cs` and `UnitTest1.cs`.
5. Add project references. `Platform.Windows` needs `Platform.Abstractions` and `Platform.Core`. `Platform.IPC` needs `Platform.Core`.
6. Implement `SystemInformationProvider` in `Platform.Windows`. Use WMI/CIM or `System.Runtime.InteropServices.RuntimeInformation` and `System.Environment` to get real data.
7. Implement `NamedPipeServer` and `NamedPipeClient` in `Platform.IPC` with typed messaging (JSON serialization).
8. Implement PowerShell AST parser in `CMDPilot.PowerShell` using `System.Management.Automation`. Add this package to Central Package Management.
9. Write unit tests asserting on real system info, real IPC connections, and real PowerShell AST extraction.
10. Ensure 0 warnings and 0 failing tests.
