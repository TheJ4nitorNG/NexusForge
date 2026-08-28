namespace Company.SysMedic.Diagnostics;

/// <summary>
/// Specifies the execution status of a diagnostic check.
/// </summary>
public enum DiagnosticStatus
{
    /// <summary>Check has not been run.</summary>
    NotRun,
    /// <summary>Check is currently running.</summary>
    Running,
    /// <summary>Check passed successfully.</summary>
    Passed,
    /// <summary>Check passed but with warnings.</summary>
    Warning,
    /// <summary>Check failed.</summary>
    Failed,
    /// <summary>Check was skipped.</summary>
    Skipped,
    /// <summary>Check encountered an error during execution.</summary>
    Error,
    /// <summary>Unknown status.</summary>
    Unknown
}

/// <summary>
/// Specifies the severity of a diagnostic finding or result.
/// </summary>
public enum DiagnosticSeverity
{
    /// <summary>Informational only.</summary>
    Information,
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
/// Specifies the category of a diagnostic check.
/// </summary>
public enum DiagnosticCategory
{
    /// <summary>General system check.</summary>
    System,
    /// <summary>Windows integrity check.</summary>
    WindowsIntegrity,
    /// <summary>Performance check.</summary>
    Performance,
    /// <summary>Storage check.</summary>
    Storage,
    /// <summary>Memory check.</summary>
    Memory,
    /// <summary>Network check.</summary>
    Network,
    /// <summary>Services check.</summary>
    Services,
    /// <summary>Startup items check.</summary>
    Startup,
    /// <summary>Security check.</summary>
    Security,
    /// <summary>Applications check.</summary>
    Applications,
    /// <summary>Hardware check.</summary>
    Hardware,
    /// <summary>Updates check.</summary>
    Updates,
    /// <summary>Drivers check.</summary>
    Drivers
}
