# SCRATCHPAD

## Track 007: Core Engines for CLI MVP

**Plan:**
1. **SysMedic DiagnosticCoordinator**:
   - Implement `IDiagnosticCoordinator` and `DiagnosticCoordinator`.
   - Take `IEnumerable<IDiagnosticCheck>`.
   - Execute concurrently with `Task.WhenAll`.
   - Aggregate `DiagnosticResult`s into a single `ScanReport`.
   - Write tests simulating slow/failing checks.
2. **CleanSlate StorageScanner**:
   - Implement `StorageScanner` (implements `IStorageScanner`).
   - Recursively traverse directories using `DirectoryInfo.EnumerateFileSystemInfos`.
   - Catch `UnauthorizedAccessException`, skip ReparsePoints/Symlinks (`Attributes.HasFlag(FileAttributes.ReparsePoint)`).
   - Report progress via `IProgress<ScanProgress>`.
   - Write integration tests using a real, temporary directory structure.
3. **CMDPilot RiskEngine**:
   - Implement `IRiskEngine` and `RiskEngine`.
   - Map known commands (e.g. `Get-Process`) to `RiskLevel.Safe`.
   - Map unknown or potentially destructive commands to `RiskLevel.Unknown` or `RiskLevel.High`.
   - If `PowerShellAstAnalyzer.DetectObfuscation` is true, force `RiskLevel.High` or `Critical`.
   - Write unit tests for various command scenarios.
4. Verify 0 errors, 0 warnings.
