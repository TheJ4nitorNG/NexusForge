# Track 009: CLI Experience for MVP

## Objective
Implement the CLI Experience (The Face) for the NexusForge MVP. This track requires wiring up the three .Cli projects using `System.CommandLine`, `Spectre.Console`, and Dependency Injection.
1. CleanSlate.Cli: Implement a `scan` command using `StorageScanner` and `CategorizationEngine` that outputs a rich table.
2. SysMedic.Cli: Implement a `scan` command using `DiagnosticCoordinator` that outputs a color-coded health report.
3. CMDPilot.Cli: Implement an `analyze` command using `PowerShellAstAnalyzer` and `RiskEngine` that displays a Risk Panel.

## Tasks
- [x] Add `System.CommandLine`, `Spectre.Console`, and `Microsoft.Extensions.Hosting` to `Directory.Packages.props`.
- [x] Add project references and package references to `CleanSlate.Cli`, `SysMedic.Cli`, and `CMDPilot.Cli`.
- [x] Ensure CPM warnings (`<NoWarn>`) and `GenerateDocumentationFile` are configured in `.Cli` project files.
- [x] Verify domain model types and enums via `read_file` (Read-Before-Write heuristic) before implementing CLI presentation logic.
- [x] Implement `CleanSlate.Cli/Program.cs` with DI and a `scan` command.
- [x] Implement `SysMedic.Cli/Program.cs` with DI and a `scan` command.
- [x] Implement `CMDPilot.Cli/Program.cs` with DI and an `analyze` command.
- [x] Run `dotnet format` immediately after writing C# code to clear `IDE0005` (unused usings) automatically.
- [x] Ensure 0 compiler warnings/errors on the first pass (XML docs, primary constructors, proper DI setup).

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track the successful implementation of production-ready, beautiful CLI interfaces that correctly consume the underlying DI-injected engines.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.