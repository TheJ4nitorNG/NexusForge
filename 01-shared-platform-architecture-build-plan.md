01 — Shared Platform Architecture & Build Plan
Below is Document #1, intended to be saved verbatim as:

/docs/00-shared-platform-architecture.md

I’ve tightened the original proposal based on the current Microsoft platform guidance. .NET 10 is currently an LTS release supported through November 14, 2028, and Microsoft recommends WinUI 3 with Windows App SDK for new native Windows desktop applications. 
M
Microsoft
+2

Shared Platform Architecture & Build Plan
Document: 00-shared-platform-architecture.md
Project Family: CMDPilot / SysMedic / IncidentKit / CleanSlate
Status: Architecture Proposal
Version: 1.0
Date: 2026-08-27
Audience: Engineering, Product, Security, DevOps, QA

1. Executive Summary
This document defines the shared technical foundation for four Windows products:

CMDPilot — AI-assisted PowerShell/CLI operations.
SysMedic — Windows diagnostics and technician toolkit.
IncidentKit — rapid incident collection and diagnostic reporting.
CleanSlate — intelligent storage analysis and digital cleanup.
These products will not be developed as four unrelated applications.

Instead, they will be built on a common platform consisting of reusable libraries, services, security controls, diagnostics providers, configuration infrastructure, logging, update mechanisms, licensing, and deployment tooling.

The goal is to create:

One engineering platform powering multiple commercial products.

This approach substantially reduces duplicated engineering effort and creates opportunities for cross-product integration.

2. Architectural Goals
2.1 Primary Goals
The platform MUST:

Run reliably on supported Windows desktop systems.
Operate correctly with and without administrator privileges where technically possible.
Clearly separate privileged and unprivileged operations.
Support both GUI and CLI applications.
Support offline operation for core functionality.
Minimize unnecessary network communication.
Protect sensitive diagnostic information.
Produce deterministic diagnostic results.
Provide machine-readable output in addition to human-readable output.
Support automated testing.
Support independent product releases while sharing platform components.
Permit future cloud integration without making cloud connectivity mandatory.
Provide a stable API between product applications and shared platform components.
2.2 Secondary Goals
The architecture SHOULD:

Support x64 and ARM64.
Permit future Windows Server tooling where appropriate.
Support enterprise deployment.
Support MSI/MSIX/package-manager distribution strategies.
Permit local AI inference in the future.
Permit multiple commercial AI providers.
Make telemetry opt-in/consent-aware.
Avoid unnecessary third-party dependencies.
2.3 Non-Goals
Version 1 of the platform will NOT attempt to:

Replace Windows Update.
Replace enterprise MDM/RMM platforms.
Replace antivirus software.
Act as a general-purpose remote administration platform.
Automatically modify critical system configuration without explicit authorization.
Automatically delete user files.
Execute arbitrary AI-generated commands without policy evaluation and user authorization.
3. Core Architectural Principle
The most important architectural decision is:

Products consume platform capabilities; products do not own platform capabilities.

For example:

CMDPilot should NOT implement its own Windows service enumeration.

SysMedic should NOT implement a second event-log parser.

IncidentKit should NOT implement a third network diagnostics library.

CleanSlate should NOT implement its own logging infrastructure.

Instead:

                        SHARED PLATFORM
                              |
        +---------------------+---------------------+
        |                     |                     |
   Diagnostics            System APIs          Infrastructure
        |                     |                     |
        +----------+----------+----------+----------+
                   |                     |
              Product SDK           Platform SDK
                   |
       +-----------+-----------+-----------+
       |           |           |           |
   CMDPilot     SysMedic   IncidentKit  CleanSlate

4. Technology Stack
4.1 Runtime
.NET 10 LTS
Primary application/runtime target:

net10.0

.NET 10 is the current LTS release and has an official support end date of November 14, 2028.

Source:

https://dotnet.microsoft.com/en-us/platform/support/policy

.NET 10 provides:

Modern C#
High-performance runtime
Native AOT capabilities
ASP.NET Core
EF Core
Generic Host
Dependency injection
Configuration
Logging
Cross-platform libraries
Strong Windows interoperability
5. Programming Languages
5.1 C#
C# is the primary language.

Use C# for:

Application logic
Platform libraries
Diagnostics
CLI applications
API services
Windows UI code
Tests
Installer/build tooling where practical
Target:

C# 14

using:

.NET 10

5.2 XAML
XAML will be used for WinUI 3 interfaces.

5.3 PowerShell
PowerShell is an integration surface, not the primary implementation language.

PowerShell modules/scripts may be provided for:

CMDPilot
SysMedic
IncidentKit
Enterprise automation
The underlying implementation remains in tested .NET libraries wherever practical.

5.4 Native C/C++
Native code is permitted only when required for:

Windows APIs unavailable through suitable .NET APIs
Hardware/driver interaction
Performance-critical functionality
Existing native APIs
Low-level Windows integration
Native code MUST NOT become the default implementation strategy.

6. Windows UI Framework
Use:

WinUI 3 + Windows App SDK

Microsoft currently recommends WinUI 3/Windows App SDK for new native Windows desktop applications.

Source:

https://learn.microsoft.com/en-us/windows/apps/winui/winui3/

The Windows App SDK provides modern Windows APIs while allowing applications to use the underlying Windows SDK when lower-level OS functionality is required.

Source:

https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/

Primary UI architecture:

WinUI 3
   |
MVVM
   |
Application Services
   |
Platform Services
   |
Windows APIs

7. Application Architecture
All GUI products should follow:

MVVM + Clean Architecture + Dependency Injection

Recommended logical layers:

Presentation
    |
Application
    |
Domain
    |
Infrastructure
    |
Windows Platform

7.1 Presentation
Contains:

XAML
Views
ViewModels
Converters
UI state
Navigation
UI-specific validation
Presentation MUST NOT directly call Windows APIs.

Bad:

MainPage.xaml.cs
    -> Win32 API

Good:

MainPage
    -> ViewModel
       -> Application Service
          -> Platform Service
             -> Windows API

8. Shared Repository Structure
Recommended monorepo:

/company-platform
│
├── README.md
├── LICENSE
├── Directory.Build.props
├── Directory.Packages.props
├── global.json
├── .editorconfig
├── .gitignore
│
├── docs/
│   ├── architecture/
│   ├── security/
│   ├── api/
│   ├── operations/
│   └── product/
│
├── src/
│   │
│   ├── Platform/
│   │   ├── Platform.Abstractions/
│   │   ├── Platform.Core/
│   │   ├── Platform.Configuration/
│   │   ├── Platform.Diagnostics/
│   │   ├── Platform.Logging/
│   │   ├── Platform.Security/
│   │   ├── Platform.Storage/
│   │   ├── Platform.Network/
│   │   ├── Platform.Windows/
│   │   ├── Platform.Processes/
│   │   ├── Platform.Services/
│   │   ├── Platform.Packaging/
│   │   ├── Platform.Updates/
│   │   ├── Platform.Licensing/
│   │   └── Platform.IPC/
│   │
│   ├── Diagnostics/
│   │   ├── Diagnostics.Abstractions/
│   │   ├── Diagnostics.Core/
│   │   ├── Diagnostics.Windows/
│   │   ├── Diagnostics.Network/
│   │   ├── Diagnostics.Storage/
│   │   ├── Diagnostics.Security/
│   │   ├── Diagnostics.Events/
│   │   └── Diagnostics.Reporting/
│   │
│   ├── Products/
│   │   ├── CMDPilot/
│   │   ├── SysMedic/
│   │   ├── IncidentKit/
│   │   └── CleanSlate/
│   │
│   └── Tools/
│       ├── BuildTools/
│       ├── PackagingTools/
│       └── TestTools/
│
├── tests/
│   ├── Platform.UnitTests/
│   ├── Platform.IntegrationTests/
│   ├── Diagnostics.UnitTests/
│   ├── Diagnostics.IntegrationTests/
│   ├── CMDPilot.Tests/
│   ├── SysMedic.Tests/
│   ├── IncidentKit.Tests/
│   └── CleanSlate.Tests/
│
└── build/
    ├── scripts/
    ├── installers/
    └── pipelines/

9. Shared Project Responsibilities
Platform.Abstractions
Contains interfaces only.

Examples:

public interface IProcessService
{
    Task<IReadOnlyList<ProcessInfo>> GetProcessesAsync(
        CancellationToken cancellationToken);
}

public interface IServiceManager
{
    Task<IReadOnlyList<ServiceInfo>> GetServicesAsync(
        CancellationToken cancellationToken);
}

public interface ISystemInformationProvider
{
    Task<SystemInformation> GetSystemInformationAsync(
        CancellationToken cancellationToken);
}

The abstractions project MUST have minimal dependencies.

10. Platform.Core
Contains shared domain-independent primitives:

Result types
Error models
Operation IDs
Correlation IDs
Time abstractions
Retry policies
Cancellation helpers
Validation
Common enums
Versioning
Example:

public sealed record OperationContext(
    Guid OperationId,
    DateTimeOffset StartedAt,
    string Product,
    string Component);

11. Platform.Configuration
Responsible for:

Application configuration
User configuration
Machine configuration
Environment variables
Configuration migration
Defaults
Configuration validation
Configuration hierarchy:

Defaults
   ↓
Machine Configuration
   ↓
User Configuration
   ↓
Environment Variables
   ↓
Command-Line Arguments

Sensitive configuration MUST NOT be stored as plaintext JSON.

12. Platform.Logging
Use:

Microsoft.Extensions.Logging

with structured logging.

Logs MUST contain, where applicable:

Timestamp
Severity
Product
Component
Operation ID
Machine ID
Event ID
Exception information
Example:

{
  "timestamp": "2026-08-27T18:30:00Z",
  "level": "Information",
  "product": "SysMedic",
  "component": "NetworkDiagnostics",
  "operationId": "8b7...",
  "eventId": 4102,
  "message": "DNS diagnostic completed"
}

Never log:

Passwords
Authentication tokens
API keys
Private keys
Full command output containing secrets
Arbitrary user file contents
13. Observability
OpenTelemetry should be used as the abstraction for future observability.

Telemetry categories:

Logs
Metrics
Traces

Default policy:

Local diagnostics first. Cloud telemetry only with explicit product/privacy policy.

The platform MUST remain functional if telemetry is disabled.

14. Platform.Diagnostics
This is one of the most important libraries in the entire ecosystem.

It provides a standardized diagnostic execution model.

public interface IDiagnosticCheck
{
    string Id { get; }

    string DisplayName { get; }

    DiagnosticCategory Category { get; }

    Task<DiagnosticResult> ExecuteAsync(
        DiagnosticContext context,
        CancellationToken cancellationToken);
}

Example checks:

SystemInformationCheck
DiskSpaceCheck
WindowsUpdateCheck
ServiceCheck
ProcessCheck
NetworkConnectivityCheck
DnsCheck
FirewallCheck
DriverCheck
EventLogCheck
StartupCheck
ScheduledTaskCheck

15. Diagnostic Result Model
Every diagnostic should produce a standardized result.

public sealed record DiagnosticResult(
    string CheckId,
    DiagnosticStatus Status,
    string Summary,
    IReadOnlyList<DiagnosticFinding> Findings,
    TimeSpan Duration);

Statuses:

Healthy
Informational
Warning
Critical
Unknown
Skipped

Example:

{
  "checkId": "network.dns",
  "status": "Critical",
  "summary": "DNS resolution failed",
  "findings": [
    {
      "severity": "Critical",
      "code": "DNS_RESOLUTION_FAILED",
      "message": "The configured DNS server did not resolve the test hostname."
    }
  ]
}

16. Diagnostic Finding Model
public sealed record DiagnosticFinding(
    string Code,
    FindingSeverity Severity,
    string Title,
    string Description,
    IReadOnlyDictionary<string, object?> Metadata);

Finding codes MUST be stable.

Example:

DISK_LOW_SPACE
DNS_RESOLUTION_FAILED
SERVICE_STOPPED
WINDOWS_UPDATE_PENDING
FIREWALL_DISABLED
DRIVER_OUTDATED
EVENTLOG_ERROR_SPIKE

Stable finding IDs allow future products and cloud systems to reason about results without depending on human-readable strings.

17. Diagnostic Execution Engine
Architecture:

DiagnosticRunner
       |
       +-- Check Registry
       |
       +-- Execution Planner
       |
       +-- Parallel Scheduler
       |
       +-- Cancellation Manager
       |
       +-- Result Aggregator
       |
       +-- Report Generator

The scheduler MUST support:

Sequential checks
Parallel checks
Dependencies
Timeouts
Cancellation
Privilege requirements
Failure isolation
A single broken diagnostic MUST NOT terminate the entire scan.

18. Privilege Model
This is critical.

The platform MUST distinguish between:

User
Administrator
SYSTEM

Most functionality should operate as the normal user.

Examples:

No elevation required
CPU information
Memory information
Basic disk usage
User-level processes
Network connectivity
DNS
Basic Windows version
User directories
Administrator may be required
Some service operations
Certain event logs
Driver information
System configuration
Protected directories
Certain security checks
SYSTEM should generally NOT be required
SYSTEM-level execution should be avoided unless a future feature absolutely requires it.

19. Elevation Architecture
Never make the entire application run elevated merely because one operation needs administrator privileges.

Instead:

Main Application
      |
      | normal privileges
      |
      +---- privileged operation requested
                    |
                    ▼
             Elevation Broker
                    |
              UAC elevation
                    |
                    ▼
             Privileged Worker
                    |
                    ▼
              Result returned

The privileged worker MUST have:

Narrow command surface
Strict input validation
No arbitrary command execution
Explicit operation allow-list
Structured IPC
Authentication/authorization between processes
20. IPC
Recommended mechanism for local product-to-service communication:

Windows Named Pipes

Use a strongly typed message protocol.

Example:

Client
  |
Named Pipe
  |
Privileged Worker

Messages:

{
  "protocolVersion": 1,
  "requestId": "abc123",
  "operation": "GetServiceDetails",
  "parameters": {
    "serviceName": "Spooler"
  }
}

Response:

{
  "protocolVersion": 1,
  "requestId": "abc123",
  "success": true,
  "result": {}
}

The protocol MUST include:

Protocol version
Request ID
Operation name
Parameters
Result/error
Timeout semantics
21. IPC Security
Named pipes MUST be protected with Windows security descriptors.

The server MUST validate:

Calling identity
Requested operation
Parameter values
Request size
Protocol version
Never permit:

ExecuteCommand("arbitrary string")

through the privileged IPC channel.

Instead expose explicit operations:

GetService
RestartService
ReadEventLog
GetDriverInformation
CollectDiagnostic

This prevents the elevation broker from becoming an arbitrary command execution primitive.

22. Local Storage
Use SQLite for structured local application state.

Potential databases:

%ProgramData%\CompanyName\Platform\platform.db
%LocalAppData%\CompanyName\CMDPilot\cmdpilot.db
%LocalAppData%\CompanyName\SysMedic\sysmedic.db
%LocalAppData%\CompanyName\IncidentKit\incidentkit.db
%LocalAppData%\CompanyName\CleanSlate\cleanslate.db

User-specific information belongs under:

%LocalAppData%

Machine-wide state belongs under:

%ProgramData%

23. SQLite Guidelines
SQLite is appropriate for:

Configuration
Scan history
Diagnostic history
User preferences
Local indexes
Command history
Application state
SQLite MUST NOT be treated as a security boundary.

Sensitive values should be protected separately.

24. Data Protection
Secrets must never be stored directly in SQLite.

Use Windows-provided protection mechanisms where appropriate, including:

DPAPI
Windows Credential Manager
Secure storage APIs
OS-protected key material
The platform should expose:

public interface ISecretStore
{
    Task SetAsync(string key, ReadOnlyMemory<byte> value);
    Task<ReadOnlyMemory<byte>?> GetAsync(string key);
    Task DeleteAsync(string key);
}

25. Network Layer
All outbound networking must go through a shared abstraction.

public interface IHttpClientFactory
{
}

Use standard .NET HTTP infrastructure.

Rules:

HTTPS only.
Certificate validation remains enabled.
No hard-coded secrets.
Timeouts required.
Cancellation required.
Retry only idempotent operations.
Exponential backoff.
Response-size limits.
Explicit user-agent identification.
26. API Architecture
The future cloud API should use:

ASP.NET Core
REST/JSON
OpenAPI

Potential architecture:

Desktop Client
      |
 HTTPS
      |
API Gateway
      |
+-----+------------------+
|                        |
Identity               Product APIs
                         |
              +----------+----------+
              |          |          |
           Licensing  Telemetry   Sync

The cloud is optional for core application functionality.

27. Cloud Database
Use:

PostgreSQL

for server-side relational data.

Candidates:

Product accounts
Organizations
Licenses
Devices
Subscriptions
Entitlements
Audit records
Aggregated telemetry
Configuration synchronization
Do not put raw diagnostic dumps into PostgreSQL.

Large artifacts should use object storage.

28. Object Storage
Potential future architecture:

Desktop
   |
   | encrypted upload
   ▼
Object Storage
   |
   +-- Incident reports
   +-- Diagnostic bundles
   +-- Crash artifacts

Use short-lived signed URLs where appropriate.

29. Licensing
The platform should have a shared licensing abstraction.

public interface ILicenseService
{
    Task<LicenseStatus> GetStatusAsync(
        CancellationToken cancellationToken);

    Task<bool> HasEntitlementAsync(
        string entitlement,
        CancellationToken cancellationToken);
}

Products should ask:

Does user have entitlement "sysmedic.pro"?

rather than implementing licensing themselves.

30. Offline Licensing
Products MUST NOT become unusable because the licensing server is temporarily unavailable.

Use cached entitlement information with a defined grace period.

Example:

Online verification
       |
       ▼
Signed entitlement
       |
       ▼
Encrypted local cache
       |
       ▼
Offline validation

The exact commercial grace period should be determined by Product/Legal.

31. Update Architecture
All products should eventually share one update framework.

Conceptually:

Update Service
      |
Manifest
      |
Version comparison
      |
Signature verification
      |
Download
      |
Integrity verification
      |
Install
      |
Rollback if necessary

Updates MUST be cryptographically signed.

The client MUST verify:

Package signature
Expected publisher
Package hash
Version metadata
Compatibility
Never execute an unsigned downloaded updater.

32. Application Packaging
Primary GUI distribution:

MSIX / Windows App SDK packaging

Windows App SDK supports packaged, unpackaged, and external-location deployment models; deployment should therefore be chosen per product rather than forcing one model everywhere.

For technician-oriented products, we may additionally require:

Portable/unpackaged distribution.

This is particularly relevant to IncidentKit.

The deployment strategy should therefore be:

CMDPilot
  → MSIX + CLI installer

SysMedic
  → MSIX + enterprise installer

IncidentKit
  → Portable executable + installer

CleanSlate
  → MSIX / consumer installer

33. Code Signing
Production binaries MUST be signed.

Signing applies to:

EXE
DLL
MSIX
Installer
PowerShell modules/scripts where appropriate
Update packages
CI/CD MUST fail release builds if signing requirements are not met.

34. Dependency Management
Use centralized package management.

Recommended:

Directory.Packages.props

Pin package versions.

Do not allow individual projects to silently drift to arbitrary dependency versions.

All dependencies must undergo:

License review
Security review
Maintenance review
35. Security Architecture
Security principles:

Least privilege
Applications receive only permissions required for the operation.

Secure by default
Dangerous functionality must require explicit activation.

Fail closed
Security failures should deny the operation rather than silently bypass controls.

No arbitrary privileged execution
The platform MUST NOT provide an API equivalent to:

RunAsAdmin(string command)

Explicit destructive operations
Operations affecting:

Files
Services
Registry
Firewall
Users
Drivers
System configuration
must be clearly identified as potentially destructive.

36. Command Execution Abstraction
CMDPilot will need command execution, but the shared platform should NOT expose unrestricted execution to every component.

Define:

public interface ICommandExecutionService
{
    Task<CommandExecutionResult> ExecuteAsync(
        CommandRequest request,
        CancellationToken cancellationToken);
}

The request should include policy metadata:

public sealed record CommandRequest(
    string Command,
    IReadOnlyList<string> Arguments,
    ExecutionPolicy Policy,
    bool RequiresElevation);

The policy engine determines whether execution is permitted.

37. Command Risk Classification
Commands should be classified:

READ_ONLY
LOW_RISK
MODIFYING
PRIVILEGED
DESTRUCTIVE
UNKNOWN

Example:

Get-Service
    READ_ONLY

Restart-Service Spooler
    MODIFYING

Remove-Item
    DESTRUCTIVE

Set-ExecutionPolicy
    PRIVILEGED / HIGH_RISK

AI-generated commands MUST NOT bypass this system.

38. AI Provider Abstraction
CMDPilot should not directly depend on one AI vendor.

Define:

public interface IAiProvider
{
    Task<AiResponse> CompleteAsync(
        AiRequest request,
        CancellationToken cancellationToken);
}

Potential providers:

Cloud Provider A
Cloud Provider B
Cloud Provider C
Local Model
Enterprise Endpoint

This lets the product change providers without redesigning CMDPilot.

39. AI Privacy Architecture
The platform MUST distinguish:

Local-only
Cloud-assisted
User-approved cloud processing
Enterprise-controlled AI

Users should know when information leaves the machine.

Sensitive command context should be minimized before transmission.

40. AI Safety Pipeline
CMDPilot architecture:

User Intent
     |
     ▼
AI Interpretation
     |
     ▼
Command Proposal
     |
     ▼
Command Parser
     |
     ▼
Risk Classifier
     |
     ▼
Policy Engine
     |
     ▼
Human Confirmation
     |
     ▼
Execution
     |
     ▼
Result Analysis

The AI model does NOT directly execute commands.

41. Audit Logging
For privileged or modifying operations, record:

Timestamp
User
Application
Operation ID
Requested operation
Risk level
Approval state
Execution result
Exit code

Do not automatically record sensitive command arguments if they may contain secrets.

42. Diagnostics Export Format
All products should be able to export structured diagnostics.

Recommended:

JSON

Optional:

HTML
CSV
TXT
ZIP

Canonical machine-readable representation:

{
  "schemaVersion": "1.0",
  "product": "SysMedic",
  "generatedAt": "2026-08-27T18:30:00Z",
  "machine": {},
  "checks": [],
  "findings": []
}

Schema versioning is mandatory.

43. Report Architecture
Reports should be generated from structured data rather than assembled directly by UI code.

Diagnostic Results
       |
       ▼
Report Model
       |
       +---- JSON Renderer
       +---- HTML Renderer
       +---- Text Renderer
       +---- CSV Renderer

This allows future reporting formats without changing diagnostic code.

44. Common Event IDs
Reserve event ID ranges.

1000-1999  Platform
2000-2999  Diagnostics
3000-3999  CMDPilot
4000-4999  SysMedic
5000-5999  IncidentKit
6000-6999  CleanSlate
7000-7999  Security
8000-8999  Updates
9000-9999  Licensing

Event IDs must remain stable across versions.

45. Testing Strategy
Every shared component requires tests.

Unit Tests
Test:

Business logic
Parsing
Classification
Validation
Configuration
Risk scoring
Result aggregation
Integration Tests
Test:

Windows APIs
Services
Event logs
Networking
SQLite
IPC
Packaging
End-to-End Tests
Test:

Install
Launch
Scan
Report
Update
Uninstall

46. Test Environment
Maintain dedicated Windows test environments.

Minimum:

Windows 11 x64
Windows 11 ARM64
Windows 10 supported target

Additional environments:

Non-admin user
Administrator
Restricted network
Offline
Low disk space
Corrupt configuration
High CPU
High memory
Large filesystem

47. CI/CD
Recommended pipeline:

Pull Request
    |
    +-- Build
    +-- Format
    +-- Static Analysis
    +-- Unit Tests
    +-- Security Scan
    +-- Dependency Audit
    |
    ▼
Merge
    |
    +-- Integration Tests
    +-- Package
    +-- Sign
    |
    ▼
Release Candidate
    |
    +-- E2E Tests
    +-- Compatibility Tests
    |
    ▼
Production Release

48. Git Branching
Use trunk-based development.

Recommended:

main

Short-lived feature branches:

feature/diagnostic-network
feature/cmdpilot-risk-engine
fix/eventlog-timeout

Avoid long-running development branches.

49. Pull Request Requirements
Every PR should contain:

Description
Motivation
Tests
Security considerations
Performance considerations
User-facing impact
Documentation impact
For privileged/system-level code:

At least one security-aware reviewer is required.

50. Static Analysis
Enable:

Nullable reference types
Treat warnings as errors for production projects where practical
Roslyn analyzers
.NET analyzers
Formatting validation
Dependency vulnerability scanning
Recommended compiler settings:

<Nullable>enable</Nullable>
<ImplicitUsings>enable</ImplicitUsings>
<AnalysisMode>Recommended</AnalysisMode>

51. Performance Requirements
Shared platform APIs should establish performance expectations.

Examples:

Startup
CLI tools:

Cold start target: < 500 ms

GUI:

Initial usable UI target: < 2 seconds

Exact targets should be validated on representative hardware.

Diagnostic checks
Individual checks should have:

Timeout
Cancellation
Maximum resource consumption
No diagnostic should be able to hang an entire scan indefinitely.

52. Cancellation
Every potentially long-running operation MUST accept:

CancellationToken

Example:

Task<DiagnosticResult> ExecuteAsync(
    DiagnosticContext context,
    CancellationToken cancellationToken);

Users must be able to cancel:

Scans
Reports
Network operations
File indexing
AI requests
Command execution where technically possible
53. Error Handling
Do not use exceptions as normal control flow.

Expected failures should become structured results.

Example:

DNS check
   |
DNS unavailable
   |
DiagnosticResult
   |
Warning/Critical

Unexpected programmer errors should still throw and be captured by the application boundary.

54. Product Isolation
Each product should reference the minimum shared libraries required.

Example:

CMDPilot
  → Platform.Core
  → Platform.Security
  → Platform.IPC
  → Platform.Logging
  → CMDPilot.Core

SysMedic
  → Platform.Core
  → Platform.Diagnostics
  → Platform.Windows
  → Platform.Logging
  → SysMedic.Core

Avoid:

CMDPilot → SysMedic

Products should not depend directly on each other.

They depend on shared platform APIs.

55. Versioning
Shared libraries use Semantic Versioning:

MAJOR.MINOR.PATCH

Breaking API changes increment MAJOR.

Backward-compatible functionality increments MINOR.

Bug fixes increment PATCH.

Internal implementation changes do not necessarily require public version changes.

56. API Compatibility
Public platform interfaces must be treated as contracts.

Before changing:

ICommandExecutionService
IDiagnosticCheck
IReportGenerator
ISecretStore

the engineering team must evaluate:

Existing consumers
Binary compatibility
Serialization compatibility
Plugin compatibility
Migration strategy
57. Plugin Architecture
Do NOT build a fully general plugin marketplace in version 1.

However, the diagnostic engine should be designed around discoverable checks.

Potential future model:

Core
 |
Diagnostic Registry
 |
+-- Built-in checks
+-- Product checks
+-- Enterprise checks
+-- Future plugins

Initial implementation should use compile-time registration.

Dynamic third-party plugins can come later after the security model has matured.

58. Shared UI Design System
Create:

CompanyName.UI

Shared controls:

StatusBadge
SeverityBadge
DiagnosticCard
MetricCard
HealthIndicator
CommandPreview
RiskBadge
ConfirmationDialog
ScanProgress
FindingList
EmptyState
ErrorState
ReportViewer
Shared colors:

Healthy   → Green
Info      → Blue
Warning   → Amber
Critical  → Red
Unknown   → Gray

The UI must remain accessible and should support dark/light themes.

59. Shared CLI Design
CLI tools should follow consistent conventions.

Example:

product command [options]

Exit codes:

0 = Success
1 = General failure
2 = Invalid arguments
3 = Permission denied
4 = Operation failed
5 = Partial success
6 = Cancelled

Machine-readable mode:

--output json

Human-readable mode:

--output text

Quiet mode:

--quiet

Verbose:

--verbose

60. Example Shared CLI Conventions
sysmedic scan
sysmedic scan --category network
sysmedic report --format html

incidentkit collect
incidentkit collect --output .\incident.zip

cmdpilot explain "Get-Service | Where-Object Status -eq Running"

Future CleanSlate CLI:

cleanslate scan
cleanslate duplicates
cleanslate large-files

61. Documentation Architecture
Every public API requires documentation.

Repository documentation:

/docs
    /architecture
    /security
    /operations
    /development
    /product
    /api

Every major architectural decision should be recorded as an ADR:

ADR-0001-use-dotnet10
ADR-0002-use-winui3
ADR-0003-shared-diagnostics-engine
ADR-0004-named-pipe-privileged-worker
ADR-0005-ai-provider-abstraction

62. Architecture Decision Records
ADRs should contain:

Title
Status
Context
Decision
Alternatives
Consequences
Date
Owners

No major architectural decision should exist only in Slack/Teams/email.

63. Threat Model
Primary threats:

T1 — Privilege escalation
Attacker attempts to abuse our privileged worker.

Mitigation:

UAC
Named pipe ACLs
Operation allow-list
Strong input validation
No arbitrary commands
Minimal privileges
T2 — AI command injection
Malicious text causes AI to generate dangerous commands.

Mitigation:

AI output treated as untrusted
Command parsing
Risk classification
Policy engine
Human confirmation
No direct model execution
T3 — Malicious diagnostic input
Corrupt log/file input attempts parser exploitation.

Mitigation:

Size limits
Timeouts
Safe parsers
Fuzz testing
No arbitrary execution
T4 — Supply-chain attack
Malicious dependency/package.

Mitigation:

Dependency pinning
Vulnerability scanning
Lock files
Package review
Signed releases
T5 — Update compromise
Attacker attempts to distribute malicious update.

Mitigation:

Code signing
Package signature validation
HTTPS
Hash validation
Key rotation strategy
64. Privacy Architecture
The default product philosophy:

The user's machine belongs to the user.

Do not collect information simply because we can.

Diagnostic data may contain:

Computer names
User names
IP addresses
Installed software
File paths
Event logs
Network configuration
Therefore:

Diagnostic collection must be treated as potentially sensitive.

Cloud uploads require explicit product policy and appropriate consent.

65. Telemetry Levels
Potential settings:

Off
Minimal
Standard
Diagnostic

Default should be the minimum necessary for product operation.

Diagnostic telemetry should never silently include raw file contents.

66. Crash Reporting
Crash reporting should capture:

Application version
OS version
Architecture
Exception type
Stack trace
Correlation ID
Sanitized environment information
Avoid:

User documents
Credentials
Full command history
Sensitive file contents
67. Shared Build Configuration
Root Directory.Build.props should enforce:

Target framework
Nullable
Analysis
Warnings
Common package metadata
Versioning
Documentation settings
Example conceptual configuration:

<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
</Project>

GUI projects may override framework requirements appropriately.

68. Build Configurations
At minimum:

Debug
Release

Additional internal configuration may include:

CI
ReleaseCandidate

Avoid proliferation of build configurations.

69. Environment Separation
Cloud infrastructure should have:

Development
Staging
Production

Never allow local developer applications to accidentally point to production services.

Configuration must make environment explicit.

70. Secrets in CI/CD
Secrets MUST be stored in the CI platform's secret management system.

Never commit:

API keys
Certificates
Private keys
Cloud credentials
Signing passwords
Database passwords

71. Release Channels
Recommended:

Canary
Beta
Stable

Example:

CMDPilot 1.4.0-canary.12
CMDPilot 1.4.0-beta.2
CMDPilot 1.4.0

Internal builds may use:

Nightly

72. Rollback
Every production update needs a rollback strategy.

Potential mechanisms:

Previous package retained
Installer rollback
Version pinning
Release channel downgrade
Emergency update block
Critical updates should support rapid disablement.

73. Feature Flags
Use feature flags for:

Experimental features
Cloud services
AI providers
Beta functionality
New diagnostic checks
Avoid using feature flags as permanent architecture.

Every feature flag needs an owner and removal plan.

74. Shared Development Standards
All engineers MUST follow:

Async APIs where appropriate
Cancellation tokens
Nullable reference types
Dependency injection
Structured logging
Unit testing
Secure coding practices
XML documentation for public APIs
Small PRs
Code review
75. MVP Platform Scope
We do NOT need the entire platform before starting products.

The initial shared platform MVP should contain:

Platform.Core
Platform.Abstractions
Platform.Logging
Platform.Configuration
Platform.Security
Platform.Windows
Platform.IPC
Diagnostics.Abstractions
Diagnostics.Core
Diagnostics.Windows
Diagnostics.Network
Diagnostics.Reporting

Delay:

Licensing cloud integration
Cloud telemetry
Plugin marketplace
Enterprise management
Advanced update orchestration

until products prove demand.

76. Initial Team Structure
For a small team:

Senior Engineer / Architect
Owns:

Shared architecture
Security
Platform interfaces
Code standards
Windows Engineer
Owns:

Windows APIs
Diagnostics
IPC
Privileged worker
UI Engineer
Owns:

WinUI
XAML
Shared design system
Product UX
Backend/Cloud Engineer
Owns:

ASP.NET Core
Licensing
Accounts
APIs
QA/Automation Engineer
Owns:

Integration testing
Windows test matrix
CI test infrastructure
One person may fill multiple roles initially.

77. First 30 Days
Week 1 — Foundation
Deliver:

Git repository
Solution structure
.NET 10
Build pipeline
Coding standards
Dependency management
Logging
Configuration
Basic test framework
Milestone:

Clean checkout → Build → Test → Pass

Week 2 — Windows Platform
Deliver:

Windows information provider
Process provider
Service provider
Disk provider
Network provider
Event log provider
Privilege detection
Elevation architecture prototype
Milestone:

platform diagnostic prototype

Week 3 — Diagnostics Engine
Deliver:

Diagnostic interfaces
Diagnostic registry
Scheduler
Cancellation
Timeout handling
Result model
Finding model
JSON serialization
HTML reporting
Milestone:

diagnostics scan → JSON + HTML

Week 4 — Product Integration
Deliver:

SysMedic prototype
IncidentKit prototype
CMDPilot command model
CleanSlate filesystem scanner prototype
Shared UI controls
First integration tests
Milestone:

Four applications can consume the same platform foundation.

78. Days 31–60
Focus:

SysMedic MVP
IncidentKit MVP
CMDPilot prototype
CleanSlate scanner

Deliver:

Production-grade diagnostics
Initial GUI
CLI interfaces
Report generation
Command-risk engine
File indexing engine
Installer prototypes
79. Days 61–90
Focus:

Commercial MVP

Deliver:

Authentication
Licensing prototype
Update mechanism
Crash reporting
Privacy controls
Documentation
Signed builds
Beta channel
Customer onboarding
First external beta users
80. Definition of Done
A platform feature is NOT done when:

“It works on my machine.”

It is done when:

Code reviewed
Unit tested
Integration tested where applicable
Logging implemented
Errors handled
Cancellation implemented
Security reviewed
Documentation updated
CI passes
No known critical vulnerabilities
Performance is acceptable
Upgrade compatibility considered
81. Critical Engineering Rule
The team must resist premature abstraction.

We should abstract things because:

Multiple products genuinely need the same capability.

Not because:

“We might need this someday.”

Good shared abstraction:

IDiagnosticCheck

Four products need diagnostics.

Bad premature abstraction:

IGenericUniversalProductFeatureProvider

for functionality only one product uses.

82. Product Dependency Graph
Target architecture:

                    Platform.Core
                         |
             +-----------+-----------+
             |           |           |
       Platform.Windows  |    Platform.Security
             |           |           |
             +-----------+-----------+
                         |
                Diagnostics.Core
                         |
          +--------------+--------------+
          |              |              |
     CMDPilot         SysMedic      IncidentKit
                                       
                         |
                    CleanSlate

CleanSlate will share platform infrastructure but will have its own specialized filesystem/indexing subsystem.

83. Long-Term Platform
Eventually:

                     COMPANY PLATFORM
                           |
       +-------------------+-------------------+
       |                   |                   |
 Diagnostics          Automation           Storage
       |                   |                   |
       +---------+---------+---------+---------+
                 |                   |
              Products            Cloud
                 |
      +----------+----------+----------+
      |          |          |          |
  CMDPilot   SysMedic  IncidentKit  CleanSlate

This gives the company a genuine technical moat.

The moat isn't merely:

“We have four Windows utilities.”

It becomes:

We have a reusable Windows operations platform that powers an ecosystem of products.

84. Final Architecture Recommendation
The final recommended stack is:

LANGUAGE
C# 14

RUNTIME
.NET 10 LTS

DESKTOP UI
WinUI 3
Windows App SDK

CLI
.NET
System.CommandLine

WINDOWS ACCESS
Windows SDK
Win32
Windows Runtime

ARCHITECTURE
Clean Architecture
MVVM
Dependency Injection

LOCAL DATA
SQLite
EF Core

SERVER
ASP.NET Core
PostgreSQL

IPC
Windows Named Pipes

LOGGING
Microsoft.Extensions.Logging

OBSERVABILITY
OpenTelemetry

SERIALIZATION
System.Text.Json

TESTING
xUnit
FluentAssertions
Integration/E2E Windows testing

CI/CD
GitHub Actions

PACKAGING
MSIX
Installer/portable distribution where appropriate

SECURITY
Code signing
DPAPI/Credential Manager
UAC
Named Pipe ACLs
Least privilege

AI
Provider-neutral abstraction
Policy/risk engine
Human approval

SOURCE CONTROL
Git/GitHub

85. Final Engineering Decision
APPROVED ARCHITECTURE

We will build a Windows-first, .NET 10 LTS shared platform with WinUI 3/Windows App SDK for native GUI applications and .NET-based CLI tools.

We will centralize:

Diagnostics
Windows integration
Privilege management
IPC
Security
Configuration
Logging
Reporting
Updates
Licensing
AI abstraction
Shared UI
Products remain independently deployable and commercially distinct.

The first implementation priority is the shared diagnostic/platform foundation, followed immediately by SysMedic and IncidentKit. CMDPilot will consume the platform while its command safety and AI layers are developed. CleanSlate will consume the common infrastructure while its filesystem analysis engine is developed independently.

The architecture is intentionally designed so that:

Every hour spent improving the platform can create value in multiple products.

That is the central engineering strategy for the product family.

Appendix A — Initial Solution Projects
Company.Platform.sln

src/
├── Platform/
│   ├── Platform.Abstractions
│   ├── Platform.Core
│   ├── Platform.Configuration
│   ├── Platform.Logging
│   ├── Platform.Security
│   ├── Platform.Storage
│   ├── Platform.Windows
│   ├── Platform.Network
│   ├── Platform.Processes
│   ├── Platform.Services
│   ├── Platform.IPC
│   ├── Platform.Updates
│   └── Platform.Licensing
│
├── Diagnostics/
│   ├── Diagnostics.Abstractions
│   ├── Diagnostics.Core
│   ├── Diagnostics.Windows
│   ├── Diagnostics.Network
│   ├── Diagnostics.Storage
│   ├── Diagnostics.Security
│   ├── Diagnostics.Events
│   └── Diagnostics.Reporting
│
└── Products/
    ├── CMDPilot
    ├── SysMedic
    ├── IncidentKit
    └── CleanSlate

Appendix B — Initial Architecture Milestones
M0  Repository + CI
M1  Platform Core
M2  Windows Providers
M3  Diagnostic Engine
M4  Reporting
M5  Privileged Worker
M6  SysMedic Prototype
M7  IncidentKit Prototype
M8  CMDPilot Prototype
M9  CleanSlate Prototype
M10 Commercial MVP
M11 Private Beta
M12 Public Release

Appendix C — Guiding Principle
Build the platform once. Build the products on top of it.

Do not optimize for the fastest first demo.

Optimize for:

fast first product → reusable foundation → fast second product → shared infrastructure → product ecosystem.

That is how we avoid building four disposable utilities and instead build a software company.

That is Document #1. The next document should be 01-cmdpilot-architecture-build-plan.md, where we get much more specific: the CMDPilot process architecture, PowerShell integration, command parser, AI provider abstraction, risk engine, approval workflow, execution sandboxing, CLI, terminal integration, data model, and the actual implementation sequence.