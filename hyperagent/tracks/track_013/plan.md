# Track 013 Plan

## Objective
Implement the System Snapshot provider and the initial suite of Windows Diagnostic Checks for SysMedic.

## Tasks
**Phase 1: SYSTEM SNAPSHOT (Platform.Windows)**
- [x] In `src/Platform/Platform.Abstractions`, define the `ISystemSnapshot` model if it doesn't exist, including OS Version, CPU Name, Total Physical Memory, and System Uptime.
- [x] In `src/Platform/Platform.Windows`, implement `ISystemInformationProvider` to gather a real `SystemSnapshot`. This should use native .NET or CIM/WMI to accurately retrieve the data.
- [x] Write unit tests to verify the data gathering logic gracefully handles potential WMI/CIM failures.

**Phase 2: IMPLEMENT DIAGNOSTIC CHECKS (SysMedic.Diagnostics.Windows)**
- [ ] Implement `LogicalDiskSpaceCheck`: Queries all local drives. Fails with a "Warning" if free space is < 15%, and "Critical" if < 5%.
- [ ] Implement `CriticalServicesCheck`: Queries the status of essential Windows services (e.g., `Winmgmt`, `Dnscache`, `LanmanWorkstation`). Fails with "Error" status if any are stopped or erroring.
- [ ] Write unit tests for both checks in `tests/SysMedic.Diagnostics.Windows.UnitTests`. Use NSubstitute to mock the WMI/FileSystem abstractions for testing, while ensuring the production code uses real Windows APIs.

**General Constraints**
- Continue skipping GUI/XAML development. Validate all logic via `SysMedic.Cli` and unit tests targeting `.NET 10`.
- Adhere to the Production-First Mandate: Zero mocks, zero placeholders in production code. Every diagnostic check must read real Windows data.
- Execute `dotnet build` and `dotnet test` frequently to validate your work. Log the raw output to `SCRATCHPAD.md` and `hyperagent/epoch_results.txt`.

## Telemetry Target
We will track implementation accuracy and completeness. Note that we will take as many turns as necessary to avoid placeholders, ensuring 100% production-ready code.
