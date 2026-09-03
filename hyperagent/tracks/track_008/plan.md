# Track 008: Concrete Capabilities for CLI MVP

## Objective
Implement the Concrete Capabilities for the CLI MVP. This track requires two major implementations:
1. SysMedic: Implement at least two concrete `IDiagnosticCheck` classes in `SysMedic.Diagnostics.Windows` (e.g., `LogicalDiskSpaceCheck` using WMI/DriveInfo and `CriticalServicesCheck` using `IServiceManager`). Ensure they return appropriately categorized `DiagnosticFinding`s.
2. CleanSlate: Implement `CategorizationEngine` in `CleanSlate.Categorization`. It should accept file metadata from the `StorageScanner` and classify files into categories (Temporary, Caches, Installers, User, Windows) based on deterministic path and extension rules.

## Tasks
- [x] Implement `LogicalDiskSpaceCheck` in `SysMedic.Diagnostics.Windows` using `System.IO.DriveInfo`.
- [x] Implement `CriticalServicesCheck` in `SysMedic.Diagnostics.Windows` using `IServiceManager`.
- [x] Write integration tests for SysMedic checks against real system state.
- [x] Implement `CategorizationEngine` and rule interfaces in `CleanSlate.Categorization`.
- [x] Write unit/integration tests for `CategorizationEngine` to ensure correct file classification based on path/extension rules.
- [x] Run `dotnet format` immediately after writing C# code to clear `IDE0005` (unused usings) automatically.
- [x] Ensure 100% test coverage and 0 compiler warnings/errors on the first pass (XML docs, primary constructors, proper cleanup).

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track the successful implementation of production-ready concrete checks and categorization logic, ensuring real system data is used and accurately processed.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.