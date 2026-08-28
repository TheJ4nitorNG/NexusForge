CleanSlate — Architecture & Build Plan
Document: 03-cleanslate-architecture-build-plan.md
Product: CleanSlate
Project Family: Windows Utility Suite
Status: Architecture Proposal
Version: 1.0
Date: 2026-08-27
Audience: Engineering, Product, Security, QA, DevOps

1. Executive Summary
CleanSlate is a Windows storage-management and cleanup application designed around one fundamental principle:

Show users exactly what is consuming their storage, explain what can safely be removed, and never delete anything without informed consent.

CleanSlate is not a registry cleaner.

It is not a fake “PC optimizer.”

It is not a scareware application that invents thousands of meaningless problems.

Instead, CleanSlate provides:

Storage visualization
Large-file discovery
Duplicate-file detection
Temporary-file cleanup
Application-cache analysis
Windows cleanup
Recycle Bin management
Download-folder analysis
Old-file discovery
Safe cleanup workflows
Cleanup previews
Deletion protection
Undo/recovery where feasible
Cleanup history
Storage trends
CLI automation for advanced users
The product's strongest selling point should be trust.

2. Product Vision
CleanSlate should answer:

“Where did all my disk space go?”

within minutes.

The ideal user experience:

C: DRIVE

512 GB Total

████████████████████░░░░  82%

Used: 420 GB
Free: 92 GB

Largest categories:

Applications       146 GB
Users               121 GB
Windows              48 GB
Games                63 GB
Other                42 GB

Then:

Potentially reclaimable

Temporary files          8.4 GB
Recycle Bin              4.1 GB
Application caches       6.8 GB
Old installers           2.7 GB

Potential total         22.0 GB

[Review Cleanup]

The user always knows what is happening.

3. Target Users
3.1 Everyday Users
People who experience:

Low disk space
Slow downloads
Large Downloads folders
Accumulated temporary files
Huge application caches
Forgotten installers
Duplicate photos/files
3.2 Gamers
Particularly useful for:

Large game libraries
Shader caches
Launcher caches
Old game installers
Mod directories
Recordings
Screenshots
3.3 Power Users
Useful features:

Advanced file search
Duplicate detection
CLI
Automation
Custom cleanup rules
Detailed storage reports
3.4 IT Technicians
Future features:

Portable mode
Storage reports
Batch cleanup
Remote execution
Standardized cleanup policies
4. Product Principles
Principle 1 — Never Invent Problems
A 400 GB drive that is 75% full is not necessarily unhealthy.

CleanSlate should report facts rather than manufacture urgency.

Principle 2 — Never Delete Without Explaining
Before deletion:

What?
Why?
How much?
Where?
Can it be recovered?

Principle 3 — Safe by Default
The default cleanup profile should contain only low-risk categories.

Principle 4 — User Files Are Sacred
Personal files should never be automatically classified as disposable merely because they are old or large.

Principle 5 — Preview Everything
Cleanup should work like:

Scan
 ↓
Review
 ↓
Select
 ↓
Preview
 ↓
Confirm
 ↓
Delete
 ↓
Verify

5. High-Level Architecture
                         CleanSlate
                            |
              +-------------+-------------+
              |                           |
         Desktop UI                    CLI
              |                           |
              +-------------+-------------+
                            |
                      Application Core
                            |
       +--------------------+--------------------+
       |                    |                    |
   Scanner              Analyzer            Cleanup Engine
       |                    |                    |
       ▼                    ▼                    ▼
 Filesystem           Categorization       Safety Engine
 Windows              Duplicate Engine     Deletion Engine
 Applications         Size Analysis        Recovery
       |
       ▼
 Storage Database

6. Recommended Technology Stack
Desktop
C# / .NET 10

UI:

WinUI 3

Core Libraries
Microsoft.Extensions.DependencyInjection
Microsoft.Extensions.Logging
Microsoft.Extensions.Configuration
System.Text.Json

Database
SQLite

Store:

Scan metadata
Cleanup history
User preferences
Custom rules
Storage snapshots
Duplicate scan metadata
Do not store unnecessary copies of filenames or file contents.

7. Solution Structure
src/Products/CleanSlate/

├── CleanSlate.App/
│
├── CleanSlate.Cli/
│
├── CleanSlate.Core/
│
├── CleanSlate.Scanner/
│
├── CleanSlate.Windows/
│
├── CleanSlate.Categorization/
│
├── CleanSlate.Duplicates/
│
├── CleanSlate.Analysis/
│
├── CleanSlate.Cleanup/
│
├── CleanSlate.Recovery/
│
├── CleanSlate.Reporting/
│
└── CleanSlate.Integration/

8. Shared Platform
CleanSlate should consume:

Platform.Core
Platform.Logging
Platform.Configuration
Platform.Security
Platform.Windows

It should also integrate with:

SysMedic
CMDPilot
IncidentKit

where useful.

9. Core Cleanup Pipeline
Filesystem Scan
       ↓
Candidate Discovery
       ↓
Classification
       ↓
Risk Assessment
       ↓
Size Calculation
       ↓
User Review
       ↓
Cleanup Plan
       ↓
Confirmation
       ↓
Deletion
       ↓
Verification
       ↓
History

10. Scanner Architecture
The scanner should support multiple scan strategies.

public interface IStorageScanner
{
    Task<StorageScanResult> ScanAsync(
        StorageScanOptions options,
        IProgress<ScanProgress>? progress,
        CancellationToken cancellationToken);
}

11. Scan Options
public sealed record StorageScanOptions
{
    public bool IncludeSystemFiles { get; init; }

    public bool IncludeHiddenFiles { get; init; }

    public bool IncludeProtectedPaths { get; init; }

    public long MinimumFileSize { get; init; }

    public bool CalculateHashes { get; init; }
}

12. Scanner Safety
The scanner must distinguish between:

Readable
Unreadable
Protected
System
Symbolic Link
Reparse Point
Offline

It must never blindly follow recursive filesystem structures.

13. Reparse Point Protection
Windows uses reparse points for:

Junctions
Symbolic links
Other filesystem features
The scanner must detect them and prevent accidental recursive traversal.

Example:

C:\A
  └── Junction → C:\A

Without protection, a scanner could recurse indefinitely.

14. Scan Performance
Large disks may contain millions of files.

The scanner should:

Stream results
Avoid loading everything into memory
Use bounded concurrency
Support cancellation
Report progress
Avoid unnecessary hashing
Cache metadata where appropriate
15. File Metadata
Initial metadata:

Path
Name
Extension
Size
Created
Modified
Accessed
Attributes
Owner where available
File type

Do not collect file contents unless required for a specific feature.

16. Storage Categories
Initial categories:

Windows
Applications
Games
Users
Documents
Downloads
Pictures
Videos
Music
Temporary
Caches
Logs
Installers
Archives
Duplicates
Unknown

Categories must be descriptive rather than judgmental.

17. Category Confidence
Classification should include confidence.

Example:

Downloads
Confidence: HIGH

Temporary cache
Confidence: HIGH

Unknown
Confidence: LOW

This prevents questionable classification from being presented as fact.

18. Classification Engine
Architecture:

File
 ↓
Path Rules
 ↓
Extension Rules
 ↓
Known Application Rules
 ↓
Windows Rules
 ↓
Metadata
 ↓
Classification

Classification should be deterministic in the MVP.

19. Category Rule Interface
public interface IStorageClassificationRule
{
    string Id { get; }

    int Priority { get; }

    ClassificationResult Evaluate(
        FileMetadata file,
        ClassificationContext context);
}

20. Rule Precedence
Example:

Explicit protected path
        ↓
Windows system path
        ↓
Known application path
        ↓
Temporary/cache rule
        ↓
User library
        ↓
Extension
        ↓
Unknown

Explicit protection always wins.

21. Protected Paths
CleanSlate should maintain a strong protected-path system.

Examples include:

Windows system directories
Boot files
Application system directories
Critical system configuration

The protection system should be conservative.

22. Cleanup Categories
Initial cleanup candidates:

Temporary files
Recycle Bin
Browser caches
Application caches
Windows temporary data
Crash dumps
Installer leftovers
Update cleanup candidates
Thumbnail caches
Log files

Each category receives a risk rating.

23. Cleanup Risk
Safe
Low
Moderate
High
Do Not Recommend

MVP cleanup should focus on:

Safe
Low

24. Cleanup Candidate
public sealed record CleanupCandidate
{
    public required string Id { get; init; }

    public required string Category { get; init; }

    public required string Description { get; init; }

    public required long EstimatedBytes { get; init; }

    public required CleanupRisk Risk { get; init; }

    public required bool Recoverable { get; init; }
}

25. Cleanup Plan
A cleanup plan should contain explicit items.

CleanSlate Cleanup Plan

Temporary files
  4.2 GB

Recycle Bin
  1.8 GB

Browser caches
  2.1 GB

Crash dumps
  0.7 GB

TOTAL
  8.8 GB

26. Cleanup Preview
Before deleting:

You are about to remove:

4.2 GB Temporary Files
1.8 GB Recycle Bin
2.1 GB Browser Cache

Total:
8.1 GB

These items will not affect your personal documents.

[Cancel]
[Clean Selected]

27. Personal File Protection
CleanSlate should default to:

DO NOT CLEAN

for:

Documents
Desktop
Pictures
Videos
Music
unless the user explicitly selects files.

28. Large File Finder
One of CleanSlate's strongest features.

Example:

Largest Files

1. game_backup.zip       43.2 GB
2. video_recording.mkv   21.8 GB
3. VM-disk.vhdx           18.4 GB
4. installer.iso          11.9 GB

The user can investigate storage without CleanSlate deciding that the files are disposable.

29. Large File Filters
Allow:

> 100 MB
> 1 GB
> 5 GB
Custom size

Also filter by:

Date
Extension
Folder
Drive

30. Duplicate Finder
Duplicate detection should be staged.

Do not immediately hash every file.

Pipeline:

File Size
   ↓
Quick Signature
   ↓
Hash
   ↓
Optional Full Verification

31. Duplicate Detection
Only files with matching:

Size

should move to the next stage.

Then compare a small sample.

Then full cryptographic hash.

This dramatically reduces unnecessary disk I/O.

32. Hashing
Recommended:

SHA-256

for final verification.

A faster non-cryptographic fingerprint may be used for preliminary grouping.

The final duplicate claim should be based on complete-content comparison.

33. Duplicate Safety
CleanSlate must never automatically choose which duplicate to delete.

Instead show:

Duplicate Group

Photo.jpg
C:\Users\...\Pictures
Created: Jan 4, 2026

Photo.jpg
D:\Backup\Pictures
Created: Jan 4, 2026

Identical:
YES

[Keep Both]
[Choose Files]

34. Old File Finder
Useful, but dangerous.

A file being old does not mean it is unnecessary.

Therefore:

Old Files

should be an analysis tool, not an automatic cleanup category.

Example:

Files not modified in 2+ years:

1,821 files
74.3 GB

Review before deleting.

35. Downloads Analyzer
Downloads are frequently a major source of clutter.

Display:

Downloads

Installers        18.2 GB
Archives           7.4 GB
Videos             4.8 GB
Documents          2.1 GB
Other              1.9 GB

Potential actions:

Review installers
Review large files
Find duplicates

Avoid automatically deleting downloads.

36. Installer Detection
Identify common installer formats:

.exe
.msi
.msix
.iso
.zip

But classification should not imply disposal.

Example:

“This appears to be an installer. It has not been modified in 14 months.”

37. Application Cache Analysis
Identify known cache locations for supported applications.

Example:

Application Caches

Browser A       2.4 GB
Application B   1.7 GB
Launcher C      0.9 GB

Each application integration should specify:

Safe to delete?
What happens afterward?
Will the application regenerate it?
Will the user be logged out?

38. Browser Cache Handling
Cache cleanup should clearly explain:

Cached website data may need to be downloaded again.

Do not delete:

Password databases
Bookmarks
Profiles
Cookies
unless the user explicitly requests those actions and understands the consequences.

39. Windows Cleanup
Potential candidates:

Temporary Windows files
Delivery Optimization cache
Windows Update cleanup candidates
Old logs
Crash dumps
Temporary installation data

Some Windows cleanup operations should be delegated to supported Windows mechanisms rather than manually deleting protected files.

40. Recycle Bin
Recycle Bin should be treated separately.

Display:

Recycle Bin

Items:
2,413

Space:
4.1 GB

Deleting these files permanently empties the selected Recycle Bin items.

The user must explicitly confirm.

41. Secure Deletion
CleanSlate should NOT advertise simplistic file overwriting as guaranteed secure deletion on modern storage.

For SSDs and modern filesystems, secure erasure is complicated.

MVP:

Normal deletion
Recycle Bin

Future:

Drive sanitization guidance

but only with careful platform-specific engineering.

42. Recovery Strategy
For normal cleanup, use the Recycle Bin when appropriate.

Possible flow:

Cleanup
 ↓
Move to Recycle Bin
 ↓
Record operation

rather than:

Permanent deletion

by default.

43. Cleanup Transaction
Cleanup operations should behave transactionally where possible.

Prepare
 ↓
Validate
 ↓
Execute
 ↓
Verify
 ↓
Commit history

If an operation fails halfway through:

Completed:
7.2 GB

Failed:
1.1 GB

Do not report the entire cleanup as successful.

44. Cleanup History
Store:

Timestamp
Cleanup ID
Categories
Items
Estimated size
Actual size
Result
Errors

Example:

Aug 27

Cleaned:
8.2 GB

Items:
1,842

Result:
Successful

45. Storage History
Track snapshots:

Aug 1     302 GB used
Aug 8     317 GB used
Aug 15    331 GB used
Aug 22    359 GB used
Aug 27    381 GB used

Then:

“Storage usage increased by 79 GB in 26 days.”

This is much more useful than simply saying “disk almost full.”

46. Storage Growth Analysis
Potential categories:

Applications
Games
Downloads
Videos
Pictures
User Data
System
Unknown

Example:

Your Applications category increased by 31 GB
during the last 14 days.

47. Storage Visualization
Primary UI:

+------------------------------------------------+
| C:                                             |
|                                                |
| ████████████████████░░░░ 82%                   |
|                                                |
| 420 GB used / 512 GB                           |
|                                                |
| Applications       146 GB                      |
| Users              121 GB                      |
| Games               63 GB                      |
| Windows             48 GB                      |
| Other               42 GB                      |
+------------------------------------------------+

48. Treemap View
Future feature:

+-------------------------------+
|           Games               |
|                               |
|        +-----------+          |
|        | Game A     |          |
|        | 42 GB      |          |
|        +-----------+----------+
|        | Game B     |          |
|        | 21 GB      |          |
+--------+------------+----------+

This gives users an intuitive visual representation of disk consumption.

49. CLI
Examples:

cleanslate scan

cleanslate scan --drive C:

cleanslate large-files --min-size 1GB

cleanslate duplicates --path "D:\Pictures"

cleanslate cleanup --preview

50. CLI JSON Output
cleanslate scan --json

Output:

{
  "drive": "C:",
  "capacity": 549755813888,
  "used": 450971566080,
  "free": 98784247808,
  "potentialCleanupBytes": 8847634432
}

This allows automation.

51. CLI Safety
Dangerous operations must require explicit flags.

For example:

cleanslate cleanup --preview

must be safe.

Actual cleanup:

cleanslate cleanup --execute

Potentially add:

--yes

only for advanced automation.

52. Automation Profiles
Future:

cleanslate profile create weekly-safe

Example profile:

Temporary Files      YES
Browser Cache        YES
Recycle Bin          NO
Downloads            NO
User Files           NO

53. Scheduled Cleanup
Scheduled cleanup should initially be limited to clearly safe categories.

Example:

Every Sunday at 3:00 AM

Clean:
Temporary files
Known application caches
Windows temporary data

Do not automatically delete personal files.

54. Configuration
Store settings in a user-specific configuration file.

Example:

{
  "cleanup": {
    "useRecycleBin": true,
    "requireConfirmation": true
  },
  "scanner": {
    "followReparsePoints": false
  }
}

55. Security Model
CleanSlate handles filesystem access and potentially elevated operations.

Security requirements:

Least privilege
Signed binaries
Secure updater
Protected-path enforcement
Safe path handling
No arbitrary elevated commands
Input validation
56. Path Traversal Protection
Every deletion request must be validated.

Do not allow crafted paths such as:

C:\safe\..\Windows

to bypass classification.

Normalize paths before policy evaluation.

57. TOCTOU Protection
Between scanning and deletion, files can change.

Therefore cleanup must revalidate:

Path
File identity
Size
Expected metadata

before deletion.

If the target changed unexpectedly:

Skip and report.

Do not blindly delete.

58. Symlink Safety
Do not follow arbitrary symbolic links when deciding what to delete.

A cleanup candidate must refer to the intended filesystem object.

59. Race Conditions
If another application modifies a file after scan:

Scan says:
2 GB

Actual file:
3 GB

CleanSlate should re-evaluate the candidate.

60. Elevation
Most scanning should run unelevated.

For protected cleanup:

User Mode
   ↓
Permission Required
   ↓
Explicit User Approval
   ↓
Elevated Helper
   ↓
Known Cleanup Operation

61. Elevated Helper
Potential:

CleanSlate.exe
      |
      | Named Pipe IPC
      ▼
CleanSlate.Elevated.exe

The elevated helper should expose a narrow API.

Never expose:

Execute arbitrary PowerShell

62. Cleanup Engine Interface
public interface ICleanupAction
{
    string Id { get; }

    CleanupRisk Risk { get; }

    Task<CleanupPreview> PreviewAsync(
        CleanupContext context,
        CancellationToken cancellationToken);

    Task<CleanupResult> ExecuteAsync(
        CleanupPlan plan,
        CancellationToken cancellationToken);
}

63. Cleanup Plugin Architecture
Future integrations:

Browser
Game Launcher
Developer Tools
Creative Applications
Cloud Storage Clients

Each integration should declare:

Name
Version
Paths
Cleanup categories
Risk
Rebuild behavior

64. Plugin Security
Third-party cleanup plugins should not receive arbitrary system privileges.

Prefer a signed, first-party rule-pack system before allowing actual third-party executable plugins.

65. Developer Mode
Power users may want:

Custom rules
Custom paths
Custom file patterns

Developer mode must be clearly separated from safe cleanup.

Example:

ADVANCED MODE

You are defining custom deletion rules.
CleanSlate cannot guarantee the safety of custom rules.

66. Reporting
Generate:

HTML
JSON
TXT

Report:

Drive
Capacity
Used
Free
Largest categories
Largest files
Cleanup candidates
Cleanup performed
Errors

67. Shareable Report
Reports should sanitize:

Username
Computer name
Full personal paths

unless explicitly requested.

68. UI Information Architecture
Primary navigation:

Overview
Storage
Cleanup
Large Files
Duplicates
History
Settings

69. Overview Screen
Example:

C: DRIVE

82% Used

420 GB / 512 GB

Potential cleanup:
8.8 GB

Largest category:
Applications

Fastest growth:
Games

[Scan Storage]
[Review Cleanup]

70. Cleanup Screen
SAFE CLEANUP

☑ Temporary files        4.2 GB
☑ Application caches     2.1 GB
☐ Recycle Bin            1.8 GB
☐ Crash dumps            0.7 GB

Potential:
8.8 GB

[Review]

71. Cleanup Explanation
Every category should have an explanation.

Example:

Temporary Files

Files created by Windows and applications
that are generally no longer required.

Risk:
LOW

Expected effect:
Applications may recreate some files.

72. Cleanup Confirmation
Use explicit language:

CleanSlate is ready to remove 8.2 GB.

Personal documents, photos, videos and installed
applications are not included.

[Cancel]
[Clean 8.2 GB]

73. Progress UI
Cleaning...

████████████████░░░░ 78%

Temporary files
3.9 GB / 4.2 GB

Items processed:
12,491

[Cancel]

Cancellation should occur at safe boundaries.

74. Completion
CLEANUP COMPLETE

Recovered:
8.17 GB

Items:
12,894

Failed:
14

The failed items were left untouched.

[View Details]
[Done]

75. Error Handling
Never silently ignore failures.

Example:

14 files could not be removed.

Reason:
Currently in use.

No action is required.

76. Logging
Events:

ScanStarted
ScanCompleted
CandidateCreated
CleanupPreviewed
CleanupApproved
CleanupStarted
ItemDeleted
ItemSkipped
CleanupCompleted
CleanupFailed

Sensitive paths should be redacted from diagnostic logs where possible.

77. Testing Strategy
Unit Tests
Test:

Classification
Risk calculation
Protected paths
Size calculations
Cleanup rules
Duplicate grouping
Storage scoring
Integration Tests
Test:

NTFS
Permissions
Reparse points
Symbolic links
Read-only files
Locked files
Large directory trees
Recycle Bin
Windows cleanup APIs
78. Destructive Test Harness
Never test deletion logic directly against developer machines.

Use a disposable filesystem test environment.

Example:

TestRoot/
├── Safe/
├── Protected/
├── Symlink/
├── ReadOnly/
├── Locked/
├── Duplicate/
└── Large/

Every destructive test operates only inside this environment.

79. Performance Targets
Normal scan:

< 60 seconds

for a typical consumer system where practical.

Large drives:

Progressive results
No UI freezing

Duplicate scans should use staged hashing to minimize disk I/O.

80. Memory Targets
The scanner must not hold millions of file records in memory.

Use:

Streaming
Paging
SQLite-backed intermediate storage
Bounded collections

81. MVP
CleanSlate MVP should include:

Windows 11
Storage overview
Drive usage
Category breakdown
Large-file finder
Temporary-file analysis
Recycle Bin analysis
Basic application-cache analysis
Safe cleanup
Cleanup preview
Cleanup history
Storage reports
CLI
JSON output
82. MVP Exclusions
Do not initially include:

Registry cleaning
RAM “optimization”
Driver cleanup
Automatic program uninstall
Aggressive browser-data deletion
Secure file shredding
Boot optimization
Automatic old-file deletion

These either add risk or distract from the core product.

83. Phase 2
Add:

Duplicate finder
Storage trends
Treemap
Download analyzer
Installer analyzer
More application integrations
Scheduled safe cleanup
Advanced reports
84. Phase 3
Add:

Technician mode
Portable edition
Custom rule packs
Advanced automation
Remote cleanup
Fleet storage reporting
85. Phase 4
Potential cloud product:

CleanSlate Endpoint
       |
       +-- Storage inventory
       +-- Cleanup policies
       +-- Fleet reporting
       +-- Remote cleanup
       +-- Capacity forecasting

This creates a legitimate enterprise market.

86. 30-Day Build Plan
Days 1–3
Build:

CleanSlate.Core
CleanSlate.Scanner
CleanSlate.Categorization
CleanSlate.App
CleanSlate.Cli

Implement:

Dependency injection
Logging
Configuration
Scanner interfaces
Days 4–7
Implement filesystem scanner:

File metadata
Directory traversal
Reparse protection
Permissions
Progress
Cancellation

Days 8–11
Implement:

Classification
Protected paths
Storage categories
Size aggregation

Days 12–15
Build storage dashboard:

Drive overview
Category visualization
Largest directories
Large files

Days 16–20
Build cleanup engine:

Cleanup candidates
Risk model
Preview
Cleanup plan
Execution
Verification
History

Days 21–24
Implement safe cleanup categories:

Temporary files
Caches
Crash dumps
Recycle Bin

Days 25–27
Build CLI:

scan
large-files
cleanup
report

Days 28–30
QA:

Filesystem edge cases
Permissions
Locked files
Symlinks
Large drives
Cancellation
Cleanup failures

87. Days 31–60
Add:

Duplicate finder
Download analyzer
Storage trends
Treemap
More cache integrations
Advanced reports

88. Days 61–90
Commercial beta:

Installer
Code signing
Licensing
Updater
Telemetry controls
Crash reporting
Documentation
Support tooling

89. Monetization Strategy
Free
Storage overview
Basic scanner
Large files
Basic cleanup

Pro
Advanced cleanup
Duplicate finder
Storage trends
Advanced application analysis
Scheduled cleanup
Advanced reports

Technician
Portable mode
CLI automation
Batch reports
Advanced cleanup controls
Technician workflows

Business
Future:

Fleet storage visibility
Central policies
Remote cleanup
Reporting
Capacity forecasting

90. Pricing Philosophy
Do not sell through fear.

Avoid:

“Your PC has 4,238 junk files!”

Instead:

“You can safely reclaim approximately 8.8 GB.”

That is factual, measurable, and defensible.

91. Competitive Differentiation
CleanSlate competes indirectly with:

PC cleaners
Storage analyzers
Duplicate finders
Windows Disk Cleanup
Manual filesystem analysis
Its differentiator:

One transparent storage-management experience that explains every cleanup decision.

92. SysMedic Integration
SysMedic can identify:

Low disk space
Storage pressure
Large growth
Potentially problematic disk utilization

CleanSlate then provides:

Detailed storage analysis
Cleanup candidates

Example:

SysMedic:
Disk space critically low.

↓
CleanSlate:
22.4 GB potentially reclaimable.

↓
User:
Reviews cleanup.

↓
CleanSlate:
Reclaims 18.7 GB.

↓
SysMedic:
Verifies improved system state.

93. CMDPilot Integration
CMDPilot can explain CLI operations.

Example:

User:
“Find out what's taking up all my disk space.”

CMDPilot:
“I can run a CleanSlate storage analysis.”

↓
CleanSlate scanner
↓
Structured results
↓
CMDPilot explanation

94. IncidentKit Integration
When storage is suspected as an incident cause:

IncidentKit
     ↓
CleanSlate inventory
     ↓
Storage report
     ↓
Incident evidence package

This makes the four-product ecosystem stronger.

95. Product Flywheel
SysMedic
   ↓
Detects storage issue
   ↓
CleanSlate
   ↓
Analyzes and cleans
   ↓
SysMedic
   ↓
Verifies improvement
   ↓
IncidentKit
   ↓
Documents incident
   ↓
CMDPilot
   ↓
Explains / assists

96. Key Metrics
Product:

Scans completed
Storage analyzed
Cleanup sessions
GB reclaimed
Duplicate files identified
Reports generated

Quality:

False classifications
Failed cleanup operations
Rollback/recovery events
Skipped files
Crash-free sessions

Business:

Free → Pro conversion
Retention
Average cleanup frequency
Paid technician accounts
Business accounts

97. Success Criteria
CleanSlate MVP succeeds when a user can:

Open CleanSlate.
See exactly how their storage is being used.
Identify unusually large files/folders.
See safe cleanup candidates.
Understand why each candidate is safe.
Preview the cleanup.
Approve it.
Recover meaningful disk space.
Verify the result.
Review the cleanup history.
98. Final Engineering Recommendation
CleanSlate should not try to win the PC-cleaner market by being more aggressive.

It should win by being more trustworthy.

The product's core architecture should therefore prioritize:

Accurate filesystem analysis.
Conservative classification.
Protected-path enforcement.
Explicit risk assessment.
Cleanup previews.
User-controlled deletion.
Revalidation before deletion.
Recovery wherever practical.
Transparent history.
Excellent storage visualization.
The strategic opportunity is larger than a cleanup utility.

CleanSlate can become the storage intelligence layer shared by the product family.

The long-term architecture should support:

                  CleanSlate
                      |
        +-------------+-------------+
        |             |             |
     Consumer      Technician    Enterprise
        |             |             |
      Cleanup       CLI/API      Fleet Storage

The ultimate product promise should remain simple:

Know what's taking your space. Know what you can remove. Stay in control.

That gives us the fourth architecture document and keeps the product deliberately differentiated from the dubious “PC optimizer” category.