# SCRATCHPAD

## Track 004: CleanSlate Foundation & Scanner Abstractions

**Plan:**
1. Scaffold CleanSlate projects in `src/Products/CleanSlate/`:
   - `CleanSlate.Core`
   - `CleanSlate.Scanner`
   - `CleanSlate.Windows`
   - `CleanSlate.Categorization`
   - `CleanSlate.Duplicates`
   - `CleanSlate.Analysis`
   - `CleanSlate.Cleanup`
   - `CleanSlate.Recovery`
   - `CleanSlate.Reporting`
   - `CleanSlate.Integration`
   - `CleanSlate.App`
   - `CleanSlate.Cli` (console)
2. Scaffold Test projects in `tests/`:
   - `CleanSlate.Scanner.UnitTests`
3. Fix test `.csproj` files for CPM and suppressions using `replace` tool.
4. Add all projects to `Company.Platform.sln`.
5. Remove default `Class1.cs` and `UnitTest1.cs`.
6. Implement core abstractions in `CleanSlate.Scanner`:
   - `StorageScanOptions.cs` (record)
   - `ScanProgress.cs` (record)
   - `StorageScanResult.cs` (record)
   - `IStorageScanner.cs` (interface)
7. Write unit tests for models in `CleanSlate.Scanner.UnitTests`.
8. Run `dotnet build` and `dotnet test` to ensure 0 errors and 0 warnings.
