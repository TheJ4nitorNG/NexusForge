# Track 010: Release Pipeline (The Ship)

## Objective
Implement the Release Pipeline (The Ship) for the NexusForge CLI MVP. This track involves configuring the CLI projects for Native AOT compilation, creating a build script, and producing the final standalone executables.

## Tasks
- [x] Modify `CleanSlate.Cli.csproj`, `SysMedic.Cli.csproj`, and `CMDPilot.Cli.csproj` to enable `<PublishAot>true</PublishAot>` and `<PublishSingleFile>true</PublishSingleFile>`.
- [x] Add explicit `<TrimmerRootAssembly>` or suppress trimming warnings (`<NoWarn>$(NoWarn);IL2026;IL2104;IL3050;IL3000;IL3053</NoWarn>`) in the `.Cli` projects to ensure `Spectre.Console`, `System.CommandLine`, and `System.Management.Automation` compile successfully under Native AOT.
- [x] Ensure `System.Text.Json` (used in IPC models) uses Source Generators if necessary, or verify it compiles safely. (It compiled cleanly with the warning suppressions for the MVP).
- [x] Create `build_release.bat` in the project root to automate `dotnet publish -c Release -r win-x64 -o dist/` for the three CLI projects.
- [x] The script must create a `dist/` directory and move the resulting `.exe` files into it.
- [x] Execute `build_release.bat` to generate the final production binaries.

## Telemetry Target
- **Implementation Accuracy & Completeness**: Track the successful implementation of Native AOT compilation and the generation of standalone executables in the `dist/` directory.
- **Placeholder Prohibition**: Zero placeholders (e.g. `// TODO`, `...`). Taking as many turns as necessary to ensure complete and perfect implementation.