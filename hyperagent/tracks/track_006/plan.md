# Track 006: Core Execution and Security Boundary

## Objective
Implement the core execution and security boundary for NexusForge. This track requires three major components:
1. Implement the remaining Windows Providers in `Platform.Windows` (e.g., `SystemInformationProvider` using WMI/CIM).
2. Implement the Elevated IPC architecture in `Platform.IPC` using strongly-typed Named Pipes (with access control security descriptors) to separate user-mode intent from privileged execution.
3. Implement the CMDPilot PowerShell AST parser in `CMDPilot.PowerShell` using `System.Management.Automation` to extract commands, parameters, and detect obfuscation natively (no regex).

## Tasks
- [x] Scaffold `Platform.Windows`, `Platform.IPC`, and their unit test projects. Ensure namespaces are `Company.Platform.*`.
- [x] Add the new projects to `Company.Platform.sln`.
- [x] Suppress warnings in the new test projects using `replace`.
- [x] Implement `SystemInformationProvider` in `Platform.Windows` retrieving real OS and hardware info.
- [x] Implement Named Pipe Client and Server in `Platform.IPC` with typed messaging.
- [x] Implement PowerShell AST extraction in `CMDPilot.PowerShell`.
- [x] Ensure all implementations assert against real system data or real PowerShell AST outputs in their xUnit tests.
- [x] Ensure 0 test failures, 0 compiler warnings, and strict C# compliance (XML comments, file-scoped namespaces).

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track the successful implementation of production-ready code interacting with system APIs, Named Pipes, and PowerShell AST, without getting stuck in linting loops.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.