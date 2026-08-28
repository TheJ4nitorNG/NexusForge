namespace Company.CMDPilot.Core;

/// <summary>
/// Specifies the type of effect a command will have.
/// </summary>
public enum EffectType
{
    /// <summary>Reads a file.</summary>
    ReadFile,
    /// <summary>Writes to a file.</summary>
    WriteFile,
    /// <summary>Deletes a file.</summary>
    DeleteFile,
    /// <summary>Creates a process.</summary>
    CreateProcess,
    /// <summary>Terminates a process.</summary>
    TerminateProcess,
    /// <summary>Reads from the registry.</summary>
    ReadRegistry,
    /// <summary>Writes to the registry.</summary>
    WriteRegistry,
    /// <summary>Starts a service.</summary>
    StartService,
    /// <summary>Stops a service.</summary>
    StopService,
    /// <summary>Restarts a service.</summary>
    RestartService,
    /// <summary>Establishes a network connection.</summary>
    NetworkConnection,
    /// <summary>Downloads a file.</summary>
    DownloadFile,
    /// <summary>Uploads a file.</summary>
    UploadFile,
    /// <summary>Changes system configuration.</summary>
    ChangeConfiguration,
    /// <summary>Changes security policy.</summary>
    ChangeSecurityPolicy,
    /// <summary>Changes a user account.</summary>
    ChangeUser,
    /// <summary>Installs software.</summary>
    InstallSoftware,
    /// <summary>An unknown effect.</summary>
    Unknown
}

/// <summary>
/// Specifies the severity of an effect.
/// </summary>
public enum EffectSeverity
{
    /// <summary>Informational severity.</summary>
    Info,
    /// <summary>Low severity.</summary>
    Low,
    /// <summary>Moderate severity.</summary>
    Moderate,
    /// <summary>High severity.</summary>
    High,
    /// <summary>Critical severity.</summary>
    Critical
}

/// <summary>
/// Specifies the overall risk level of a command.
/// </summary>
public enum RiskLevel
{
    /// <summary>Safe to execute.</summary>
    Safe,
    /// <summary>Low risk.</summary>
    Low,
    /// <summary>Moderate risk.</summary>
    Moderate,
    /// <summary>High risk.</summary>
    High,
    /// <summary>Critical risk.</summary>
    Critical,
    /// <summary>Unknown risk.</summary>
    Unknown
}

/// <summary>
/// Specifies the privilege level required for execution.
/// </summary>
public enum PrivilegeLevel
{
    /// <summary>Standard user privileges.</summary>
    User,
    /// <summary>Administrator privileges.</summary>
    Administrator,
    /// <summary>SYSTEM privileges.</summary>
    System
}

/// <summary>
/// Represents a specific effect of a command.
/// </summary>
/// <param name="Type">The type of the effect.</param>
/// <param name="Description">A human-readable description of the effect.</param>
/// <param name="Severity">The severity of the effect.</param>
public sealed record CommandEffect(
    EffectType Type,
    string Description,
    EffectSeverity Severity);

/// <summary>
/// Represents a proposed command ready for risk analysis and user approval.
/// </summary>
public sealed record CommandProposal
{
    /// <summary>
    /// Gets the unique identifier for this proposal.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the target shell for execution (e.g., powershell, cmd).
    /// </summary>
    public required string Shell { get; init; }

    /// <summary>
    /// Gets the raw command text to be executed.
    /// </summary>
    public required string CommandText { get; init; }

    /// <summary>
    /// Gets the human-readable explanation of what the command does.
    /// </summary>
    public required string Explanation { get; init; }

    /// <summary>
    /// Gets the evaluated risk level of the command.
    /// </summary>
    public required RiskLevel RiskLevel { get; init; }

    /// <summary>
    /// Gets the privilege level required to execute the command.
    /// </summary>
    public required PrivilegeLevel RequiredPrivilege { get; init; }

    /// <summary>
    /// Gets the list of identified effects this command will have.
    /// </summary>
    public required IReadOnlyList<CommandEffect> Effects { get; init; }
}
