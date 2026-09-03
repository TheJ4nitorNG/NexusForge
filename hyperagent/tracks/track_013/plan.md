# Track 013 Plan

## Objective
Implement the safe cleanup and deletion engine for CleanSlate (`CleanSlate.Cleanup`).

## Tasks
**Phase 1: Core Models & Interfaces (`CleanSlate.Cleanup`)**
- [x] Create `CleanupAction.cs` (representing a single file deletion action).
- [x] Create `CleanupProfile.cs` (configuring what categories are active).
- [x] Create `ICleanupEngine.cs` defining:
    - `Task<IReadOnlyList<CleanupAction>> PreviewCleanupAsync(CleanupProfile profile, CancellationToken token)`
    - `Task<CleanupResult> ExecuteCleanupAsync(IReadOnlyList<CleanupAction> actions, CancellationToken token)`

**Phase 2: The Cleanup Engine Implementation (`CleanupEngine.cs`)**
- [x] Implement the hardcoded directory blacklist check (`C:\Windows`, `C:\Program Files`, etc.).
- [x] Implement safe file deletion with exception tolerance (locked/in-use log files skip without crashing).
- [x] Integrate a progressive deletion reporter.

**Phase 3: Rigorous Safeguard Testing (`CleanSlate.Cleanup.UnitTests`)**
- [x] Create a new test project `tests/CleanSlate.Cleanup.UnitTests`.
- [x] Write unit tests verifying:
    - Dry-run matches execution size.
    - Blacklist Block throws `CriticalSecurityException` and deletes 0 files.
    - Deletion of temporary test files is completed successfully.

## Telemetry Target
Track implementation accuracy and completeness. Ensure 100% production-ready code with zero placeholders or mocks. Raw build/test output will be logged to `SCRATCHPAD.md` and `hyperagent/epoch_results.txt`.
