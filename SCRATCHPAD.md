# SCRATCHPAD

## Track 008: Concrete Capabilities for CLI MVP

**Plan:**
1. **SysMedic Diagnostic Checks**:
   - Verify `tests/SysMedic.Diagnostics.Windows.UnitTests` exists. Scaffold if not, configure `<NoWarn>`, add references.
   - Implement `LogicalDiskSpaceCheck` in `SysMedic.Diagnostics.Windows` using `System.IO.DriveInfo`. Checks system drive free space percentage.
   - Implement `CriticalServicesCheck` in `SysMedic.Diagnostics.Windows` using `IServiceManager`.
   - Write integration tests for both.
   - Run `dotnet format` immediately.
   - Run `dotnet test`.

2. **CleanSlate Categorization Engine**:
   - Verify `tests/CleanSlate.Categorization.UnitTests` exists. Scaffold if not, configure `<NoWarn>`, add references.
   - Define `FileMetadata` (Path, Extension, Size) in `CleanSlate.Core` or `CleanSlate.Categorization`.
   - Define `IStorageClassificationRule` and `ClassificationResult`.
   - Implement `PathRule`, `ExtensionRule`, and `CategorizationEngine`.
   - Write unit tests evaluating file classifications (Temporary, Caches, Installers, User, Windows).
   - Run `dotnet format` immediately.
   - Run `dotnet test`.

3. **Verify Compliance**:
   - Ensure 0 errors, 0 warnings.
