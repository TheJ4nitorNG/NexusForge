# Track 002: CMDPilot Core Domain & Command Model

## Objective
Establish the foundational projects and domain models for CMDPilot (based on 01-cmdpilot-architecture-build-plan.md). This includes defining the core models (CommandProposal, CommandEffect, RiskResult, etc.) and setting up the projects for CMDPilot.Core, CMDPilot.Commands, CMDPilot.Risk, CMDPilot.Execution, CMDPilot.PowerShell, CMDPilot.Cli, and CMDPilot.App.

## Tasks
- [x] Scaffold CMDPilot projects (`CMDPilot.Core`, `CMDPilot.Commands`, `CMDPilot.Risk`, `CMDPilot.Execution`, `CMDPilot.PowerShell`, `CMDPilot.Cli`, `CMDPilot.App`) and their test projects.
- [x] Add the newly scaffolded CMDPilot projects to the solution `Company.Platform.sln`.
- [x] Implement `CommandProposal`, `CommandEffect`, `EffectType`, `EffectSeverity`, `RiskLevel`, and `PrivilegeLevel` inside `CMDPilot.Core`.
- [x] Implement `RiskResult` and `ExecutionResult` (if different from Platform.Abstractions) inside `CMDPilot.Core`.
- [x] Ensure 0 failing tests (write unit tests for the core models).
- [x] Ensure `TreatWarningsAsErrors=true` compliance by adding XML comments and standard C# formatting from the first attempt.

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track creation of production-ready core abstractions without getting trapped in linting loops.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.