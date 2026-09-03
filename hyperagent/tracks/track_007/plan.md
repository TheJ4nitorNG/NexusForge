# Track 007: Core Engines for CLI MVP

## Objective
Implement the Core Engines for the NexusForge CLI MVP. This track requires three major implementations:
1. SysMedic: Implement `DiagnosticCoordinator` in `SysMedic.Diagnostics` that accepts an `IEnumerable<IDiagnosticCheck>`, executes them concurrently with timeout handling, and returns an aggregated result.
2. CleanSlate: Implement `StorageScanner` in `CleanSlate.Scanner` that traverses local directories safely (catching `UnauthorizedAccessException` and skipping Reparse Points/Symlinks to prevent loops), reporting progress via `IProgress<ScanProgress>`.
3. CMDPilot: Implement `RiskEngine` in `CMDPilot.Risk` that takes the extracted PowerShell AST commands/obfuscation flags and returns a deterministic `RiskResult`.

## Tasks
- [x] Implement `DiagnosticCoordinator` in `SysMedic.Diagnostics`.
- [x] Write integration/unit tests for `DiagnosticCoordinator` simulating slow and failing checks.
- [x] Implement `StorageScanner` in `CleanSlate.Scanner`. Ensure strict directory traversal safety constraints.
- [x] Write integration/unit tests for `StorageScanner` using a temporary, isolated, real directory structure.
- [x] Implement `RiskEngine` in `CMDPilot.Risk`.
- [x] Write unit tests for `RiskEngine` verifying deterministic mappings of commands to risk levels.
- [x] Ensure 100% test coverage with real filesystem/logic tests.
- [x] Strictly adhere to C# compliance (XML docs, primary constructors, no unused usings) on the *FIRST pass* to avoid linting errors.

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track the successful implementation of production-ready code with safe filesystem enumeration, robust asynchronous cancellation, and perfect C# compliance.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.