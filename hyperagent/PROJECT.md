# Project Goal
A shared engineering platform powering multiple commercial Windows products (CMDPilot, SysMedic, IncidentKit, CleanSlate). This platform consists of reusable libraries, services, security controls, diagnostics providers, configuration infrastructure, logging, update mechanisms, licensing, and deployment tooling to reduce duplicated effort and create cross-product integration.

# Tech Stack
- Language: C#
- Runtime: .NET 10 LTS
- UI: WinUI 3 / Windows App SDK
- CLI: System.CommandLine / .NET
- PowerShell: PowerShell 7.x + System.Management.Automation
- Web API: ASP.NET Core
- ORM: EF Core
- Database: SQLite (Local), PostgreSQL (Server)
- IPC: Named Pipes / local RPC
- Serialization: System.Text.Json
- Logging: Microsoft.Extensions.Logging + OpenTelemetry
- Config: Microsoft.Extensions.Configuration/Options
- DI/Hosting: Microsoft.Extensions.Hosting
- Testing: xUnit + FluentAssertions, NSubstitute
- CI/CD: GitHub Actions
- Packaging: MSIX + signed installers, WiX
- AI Abstraction: Provider-neutral internal AI interface
- Source Control: Git + GitHub
- Docs: Markdown + generated API docs
