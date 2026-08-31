# SCRATCHPAD

## Track 010: Release Pipeline (The Ship)

**Plan:**
1. Modify `src/Products/CleanSlate/CleanSlate.Cli/CleanSlate.Cli.csproj`.
   - Add `<PublishAot>true</PublishAot>`
   - Add `<PublishSingleFile>true</PublishSingleFile>`
   - Add IL warnings to `NoWarn` (`IL2026;IL2104;IL3050`)
2. Modify `src/Products/SysMedic/SysMedic.Cli/SysMedic.Cli.csproj`.
3. Modify `src/Products/CMDPilot/CMDPilot.Cli/CMDPilot.Cli.csproj`.
4. Create `build_release.bat`.
   - `mkdir dist` (if not exists)
   - `dotnet publish src/Products/CleanSlate/CleanSlate.Cli/CleanSlate.Cli.csproj -c Release -r win-x64 -o dist/`
   - `dotnet publish src/Products/SysMedic/SysMedic.Cli/SysMedic.Cli.csproj -c Release -r win-x64 -o dist/`
   - `dotnet publish src/Products/CMDPilot/CMDPilot.Cli/CMDPilot.Cli.csproj -c Release -r win-x64 -o dist/`
5. Execute `build_release.bat` via shell command.
6. Verify output in `dist/`.
