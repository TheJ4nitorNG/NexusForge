# Track 011 Plan

## Objective
Initialize and implement the core engines for NexusForge: the Diagnostics Engine and the CMDPilot Risk Engine.

## Constraints & Context
1. We are skipping GUI/XAML development due to SDK compiler issues. All testing and execution must be validated against the .NET 10 core libraries and the CLI projects (`SysMedic.Cli` and `CMDPilot.Cli`).
2. Adhere strictly to the architecture defined in `docs/00-shared-platform-architecture.md`, `docs/01-cmdpilot-architecture-build-plan.md`, and `docs/02-sysmedic-architecture-build-plan.md`.
3. YOU MUST adhere to the Production-First Mandate: Zero mocks, zero `// TODO`s, zero placeholders. Every implementation must be complete and backed by a passing xUnit test.
4. Execute `dotnet build` and `dotnet test` frequently to validate your work, logging the raw output to `SCRATCHPAD.md` as mandated in Epoch 1 rules.

## Tasks
### Phase 1: Diagnostics Engine (Platform.Diagnostics)
- [ ] In `src/Platform/Platform.Abstractions`, define the core diagnostic interfaces: `IDiagnosticCheck`, `DiagnosticResult`, `DiagnosticStatus`, `DiagnosticFinding`, and `DiagnosticContext`.
- [ ] Implement a `DiagnosticCoordinator` (in `SysMedic.Diagnostics`) that can accept a list of `IDiagnosticCheck`s, execute them, and aggregate the `DiagnosticResult`s into a system health report.
- [ ] Write comprehensive unit tests for the coordinator in `tests/SysMedic.Diagnostics.UnitTests` to verify it handles passing checks, failing checks, and timeouts correctly.
- [ ] Verify Phase 1 with `dotnet test` before proceeding to Phase 2.

### Phase 2: CMDPILOT Risk Engine (CMDPilot.Risk)
- [ ] In `src/Products/CMDPilot/CMDPilot.Commands`, define the core models: `CommandProposal`, `RiskLevel`, `PrivilegeLevel`, and `CommandEffect`.
- [ ] Implement a deterministic `RiskEngine` (in `CMDPilot.Risk`) that evaluates a `CommandProposal`.
- [ ] Write comprehensive unit tests in `tests/CMDPilot.Risk.UnitTests` passing at least 5 different command profiles to verify the classification logic is deterministic and accurate.

## Telemetry Target
**Goal:** Track implementation accuracy and completeness. 
*Note:* We will take as many turns as necessary to ensure 100% production-ready code with absolutely zero placeholders or mocks. Raw output of `dotnet build` and `dotnet test` will be logged to `SCRATCHPAD.md` and `hyperagent/epoch_results.txt`.