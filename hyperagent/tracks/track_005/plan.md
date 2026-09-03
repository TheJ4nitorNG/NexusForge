# Track 005: Implement Platform Providers (IProcessService & IServiceManager)

## Objective
Implementing the Platform Providers: Wiring up `IProcessService` and `IServiceManager` to actual Windows APIs (WMI/CIM/Win32) as specified by the user.

## Tasks
- [x] Scaffold `Platform.Processes` and `Platform.Services` projects inside `src/Platform/`, along with their respective unit test projects `tests/Platform.Processes.UnitTests` and `tests/Platform.Services.UnitTests`. **Ensure namespaces correctly match `Company.Platform.*`**.
- [x] Add newly scaffolded projects to `Company.Platform.sln`.
- [x] Add necessary project references (e.g., referencing `Platform.Abstractions` and `Platform.Core`).
- [x] Apply aggressive test suppression configurations to test `.csproj` files (`<NoWarn>$(NoWarn);CA1707;CS1591;IDE0058;IDE0008;IDE0065</NoWarn>`) using the `replace` tool.
- [x] Implement `IProcessService` inside `Platform.Processes` (using `System.Diagnostics.Process` or WMI/CIM).
- [x] Implement `IServiceManager` inside `Platform.Services` (using `System.ServiceProcess.ServiceController` or WMI/CIM).
- [x] Write unit/integration tests for both providers to verify real Windows API interaction (without mock data).
- [x] Ensure 0 failing tests and 0 warnings. Maintain strict C# XML comment compliance and formatting.

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track the successful implementation of production-ready, non-mocked Windows API interactions.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.