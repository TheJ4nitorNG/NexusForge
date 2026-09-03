# Track 012 Plan

## Objective
Initialize and implement the core engines for NexusForge: the Diagnostics Engine and the CMDPilot Risk Engine.

## Tasks
**Phase 1: Diagnostics Engine (Platform.Diagnostics)**
- [x] In `src/Platform/Platform.Abstractions`, define the core diagnostic interfaces: `IDiagnosticCheck`, `DiagnosticResult`, `DiagnosticStatus`, `DiagnosticFinding`, and `DiagnosticContext`.
- [x] Implement a `DiagnosticCoordinator` (in `SysMedic.Diagnostics`) that can accept a list of `IDiagnosticCheck`s, execute them, and aggregate the `DiagnosticResult`s into a system health report.
- [x] Write comprehensive unit tests for the coordinator in `tests/SysMedic.Diagnostics.UnitTests` to verify it handles passing checks, failing checks, and timeouts correctly.
- [x] Execute Phase 1 completely and verify with `dotnet test` before moving to Phase 2.

**Phase 2: CMDPILOT Risk Engine (CMDPilot.Risk)**
- [x] In `src/Products/CMDPilot/CMDPilot.Commands`, define the core models: `CommandProposal`, `RiskLevel`, `PrivilegeLevel`, and `CommandEffect`.
- [x] Implement a deterministic `RiskEngine` (in `CMDPilot.Risk`) that evaluates a `CommandProposal`. Assign risk based on factors like destructiveness, privilege requirements, and network access.
- [x] Write comprehensive unit tests in `tests/CMDPilot.Risk.UnitTests` passing at least 5 different command profiles to verify the classification logic is deterministic and accurate.

**General Constraints**
- Skip GUI/XAML development due to SDK compiler issues. Validate all testing/execution against .NET 10 core libraries and CLI projects (`SysMedic.Cli` and `CMDPilot.Cli`).
- Adhere strictly to the architecture defined in `docs/00-shared-platform-architecture.md`, `docs/01-cmdpilot-architecture-build-plan.md`, and `docs/02-sysmedic-architecture-build-plan.md`.
- Adhere to the Production-First Mandate: Zero mocks, zero `// TODO`s, zero placeholders. Every implementation must be complete and backed by a passing xUnit test.
- Execute `dotnet build` and `dotnet test` frequently to validate your work. Log the raw output to `SCRATCHPAD.md` and `hyperagent/epoch_results.txt`.

## Telemetry Target
We will track implementation accuracy and completeness. Note that we will take as many turns as necessary to avoid placeholders, ensuring 100% production-ready code.
