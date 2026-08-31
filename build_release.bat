@echo off
setlocal

echo Creating dist directory...
if not exist "dist" mkdir dist

echo Building CleanSlate.Cli...
dotnet publish src/Products/CleanSlate/CleanSlate.Cli/CleanSlate.Cli.csproj -c Release -r win-x64 -o dist/

echo Building SysMedic.Cli...
dotnet publish src/Products/SysMedic/SysMedic.Cli/SysMedic.Cli.csproj -c Release -r win-x64 -o dist/

echo Building CMDPilot.Cli...
dotnet publish src/Products/CMDPilot/CMDPilot.Cli/CMDPilot.Cli.csproj -c Release -r win-x64 -o dist/

echo Release Pipeline Complete. Standalone executables are in the dist/ folder.
endlocal